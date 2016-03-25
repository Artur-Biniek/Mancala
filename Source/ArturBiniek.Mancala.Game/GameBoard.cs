using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ArturBiniek.Mancala.Game
{
    public class GameBoard : GameStateBase
    {
        public const int SEEDS_PER_BUCKET = 4;
        public const int BUCKETS_PER_PLAYER = 6;
        public const int MAXSEEDS = 2 * SEEDS_PER_BUCKET * BUCKETS_PER_PLAYER;
        public const int TOTAL = 2 * BUCKETS_PER_PLAYER + 2;
        public const int M1 = BUCKETS_PER_PLAYER;
        public const int M2 = 2 * BUCKETS_PER_PLAYER + 1;

        private Random _rnd = new Random();

        private Player _currentPlayer;

        private int[,] _hashBase = new int[TOTAL, MAXSEEDS];
        private Dictionary<Player, int> _hashBasePlayer = new Dictionary<Player, int>(2);
        private int _positionHash;

        protected override Player CurentPlayer
        {
            get { return _currentPlayer; }
        }

        protected override bool IsTerminal
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override IEnumerable<Move> Moves
        {
            get
            {
                return GenerateMoves();
            }
        }

        protected override int Evaluate()
        {
            throw new NotImplementedException();
        }

        internal override void MakeMove(Move move)
        {
            throw new NotImplementedException();
        }

        internal override void UndoMove(Move move)
        {
            throw new NotImplementedException();
        }

        private readonly int[] _bucktes;
        private readonly int[] _opposite;

        public GameBoard(Player current, int[] p1buckets, int p1mancala, int[] p2bukets, int p2mancala)
        {
            _bucktes = new int[TOTAL];
            _currentPlayer = current;

            for (int i = 0; i < BUCKETS_PER_PLAYER; i++)
            {
                _bucktes[i] = p1buckets[i];
                _bucktes[M1 + 1 + i] = p2bukets[i];
            }

            _bucktes[M1] = p1mancala;
            _bucktes[M2] = p2mancala;

            _opposite = new int[TOTAL];
            var ind = M2 - 1;
            for (int i = 0; i < BUCKETS_PER_PLAYER; i++)
            {
                _opposite[i] = ind;
                _opposite[ind] = i;
                ind--;
            }

            InitHash();
        }

        public override string ToString()
        {
            var l1 = string.Format("{0} [{1}]", string.Join("-", _bucktes.Take(BUCKETS_PER_PLAYER)), _bucktes[M1]);
            var l2 = string.Format("{0} [{1}]", string.Join("-", _bucktes.Skip(BUCKETS_PER_PLAYER + 1).Take(BUCKETS_PER_PLAYER)), _bucktes[M2]);

            return string.Format("{0}  |  {1}  | Current Player: {2}  | Hash: {3}", l1, l2, _currentPlayer, _positionHash);
        }

        private void InitHash()
        {
            for (int i = 0; i < TOTAL; i++)
            {
                for (int k = 0; k < MAXSEEDS; k++)
                {
                    _hashBase[i, k] = Rand32();
                }
            }

            _hashBasePlayer[Player.One] = Rand32();
            _hashBasePlayer[Player.Two] = Rand32();

            RecalculateHash();
        }

        private void RecalculateHash()
        {
            _positionHash = 0;

            for (int i = 0; i < TOTAL; i++)
            {
                HashInOut(i);
            }

            HashInOut(_currentPlayer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HashInOut(Player player)
        {
            _positionHash ^= _hashBasePlayer[player];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HashInOut(int index)
        {
            _positionHash ^= _hashBase[index, _bucktes[index]];
        }

        private int Rand32()
        {
            byte[] rnds = new byte[4];

            _rnd.NextBytes(rnds);

            return rnds[0] | rnds[1] << 8 | rnds[2] << 16 | rnds[3] << 23;
        }

        private IEnumerable<Move> GenerateMoves()
        {
            var normalMoves = new List<Move>();
            var repeatMoves = new List<Move>();
            var captureMove = new List<Move>();

            int start, ownMancala, opponentMancala;

            if (CurentPlayer == Player.One)
            {
                start = M1 - 1;
                ownMancala = M1;
                opponentMancala = M2;
            }
            else
            {
                start = M2 - 1;
                ownMancala = M2;
                opponentMancala = M1;
            }

            var end = start - BUCKETS_PER_PLAYER + 1;


            for (int i = start; i >= end; i--)
            {
                var hand = _bucktes[i];

                if (hand == 0)
                {
                    continue;
                }
                else if (hand + i == ownMancala)
                {
                    var compund = new CompoundMove(i);

                    // modify buckets
                    // compound.AddChildren(...)
                    // resotre buckets

                    repeatMoves.Add(compund);
                }
                else if (hand + i < ownMancala && _bucktes[hand + i] == 0 && _bucktes[_opposite[hand + i]] != 0)
                {
                    captureMove.Add(new CaptureMove(i));
                }
                else
                {
                    normalMoves.Add(new NormalMove(i));
                }
            }

            return repeatMoves.Concat(captureMove).Concat(normalMoves);
        }

    }
}
