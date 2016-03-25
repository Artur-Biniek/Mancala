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

        private Player _currentPlayer;

        private int[,] _hashBase = new int[TOTAL, MAXSEEDS];
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

        protected override IEnumerable<Move> Moves
        {
            get
            {
                throw new NotImplementedException();
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

            InitHash();
        }

        public override string ToString()
        {
            var l1 = string.Format("{0} [{1}]", string.Join("-", _bucktes.Take(BUCKETS_PER_PLAYER)), _bucktes[M1]);
            var l2 = string.Format("{0} [{1}]", string.Join("-", _bucktes.Skip(BUCKETS_PER_PLAYER+1).Take(BUCKETS_PER_PLAYER)), _bucktes[M2]);

            return string.Format("{0}  |  {1}  | Current Player: {2}  | Hash: {3}", l1, l2, _currentPlayer, _positionHash);
        }

        private void InitHash()
        {
            var rnd = new Random();

            for (int i = 0; i < TOTAL; i++)
            {
                for (int k = 0; k < MAXSEEDS; k++)
                {
                    _hashBase[i, k] = rnd.Next();
                }
            }

            for (int i = 0; i < TOTAL; i++)
            {
                HashInOut(i);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void HashInOut(int index)
        {
            _positionHash ^= _hashBase[index, _bucktes[index]];
        }
    }
}
