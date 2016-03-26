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

        public override Player CurentPlayer
        {
            get { return _currentPlayer; }
        }

        public override int PosKey
        {
            get { return _positionHash; }
        }

        public override bool IsTerminal
        {
            get
            {
                var p1Empty = true;
                for (int i = 0; i < M1; i++)
                {
                    if (_bucktes[i] != 0)
                    {
                        p1Empty = false;
                        break;
                    }
                }

                if (p1Empty) return true;

                for (int i = M1 + 1; i < M2; i++)
                {
                    if (_bucktes[i] != 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public override IEnumerable<Move> Moves
        {
            get
            {
                var moves = GenerateMoves().ToList();

                return moves;
            }
        }

        public override int Evaluate()
        {
            var p1beans = _bucktes[M1];
            var p2beans = _bucktes[M2];

            for (int i = 0; i < M1; i++)
            {
                p1beans += _bucktes[i];
            }

            for (int i = M1 + 1; i < M2; i++)
            {
                p2beans += _bucktes[i];
            }

            var score = p1beans - p2beans;

            return _currentPlayer == Player.One ? score : -score;
        }

        public override void MakeMove(Move move)
        {
            var baseMove = move as Game.MoveBase;
            var norm = move as NormalMove;
            var capture = move as CaptureMove;
            var compound = move as CompoundMove;

            baseMove.StoreHistory(_bucktes, _currentPlayer, _positionHash);

            var landingBucketIndex = TransferBeans(baseMove.BucketIndex);

            if (norm != null)
            {
                _currentPlayer = NextPlayer(CurentPlayer);
            }
            else if (capture != null)
            {
                var oppositeBucketIndex = _opposite[landingBucketIndex];
                var captucredBeans = _bucktes[oppositeBucketIndex];

                _bucktes[oppositeBucketIndex] = 0;
                _bucktes[landingBucketIndex] += captucredBeans;

                _currentPlayer = NextPlayer(CurentPlayer);
            }
            else if (compound != null)
            {
                MakeMove(compound.Kid);
            }

            RecalculateHash();
        }

        public override void UndoMove(Move move)
        {
            (move as Game.MoveBase).RetoreHistory(ref _bucktes, ref _currentPlayer, ref _positionHash);
        }

        private readonly int[] _opposite;
        private int[] _bucktes;

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

        public MoveBase FindMove()
        {
            var controller = new SearchController(POSINF, 2500, 10000);

            var res = (MoveBase)SearchPosition(controller);

            return res;
        }

        public override string ToString()
        {
            var l1 = string.Format("{0} [{1}]", string.Join("-", _bucktes.Take(BUCKETS_PER_PLAYER)), _bucktes[M1]);
            var l2 = string.Format("{0} [{1}]", string.Join("-", _bucktes.Skip(BUCKETS_PER_PLAYER + 1).Take(BUCKETS_PER_PLAYER)), _bucktes[M2]);

            return string.Format("{0}  |  {1}  | Current Player: {2}  | Hash: {3} | Terminal: {4} | Eval: {5}", l1, l2, _currentPlayer, _positionHash, IsTerminal, Evaluate());
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

        private IEnumerable<Game.MoveBase> GenerateMoves()
        {
            var normalMoves = new List<Game.MoveBase>();
            var repeatMoves = new List<Game.MoveBase>();
            var captureMove = new List<Game.MoveBase>();

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
                    // store buckets
                    var backup = new int[TOTAL];
                    Array.Copy(_bucktes, backup, _bucktes.Length);

                    // modify buckets
                    TransferBeans(i);

                    var kids = GenerateMoves().ToList();

                    if (kids.Count > 0)
                    {
                        foreach (var kid in GenerateMoves())
                        {
                            repeatMoves.Add(new CompoundMove(i, kid));
                        }
                    }
                    else
                    {
                        normalMoves.Add(new NormalMove(i));
                    }

                    // resotre buckets
                    _bucktes = backup;


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

        private int TransferBeans(int index)
        {
            var avoidMancala = CurentPlayer == Player.One ? M2 : M1;
            var hand = _bucktes[index];
            var current = index;

            _bucktes[index] = 0;

            while (hand > 0)
            {
                current++;

                if (current >= TOTAL)
                {
                    current %= TOTAL;
                }

                if (current == avoidMancala)
                {
                    continue;
                }

                _bucktes[current]++;

                hand--;
            }

            return current;
        }

    }
}
