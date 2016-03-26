using System;
using System.Collections.Generic;
using System.Linq;

namespace ArturBiniek.Mancala.Game
{
    public abstract class GameStateBase
    {
        protected const int PV_SIZE = 10000;
        protected const int TIME_LIMIT = 2500;
        protected const int NEGINF = int.MinValue + 100;
        protected const int POSINF = int.MaxValue - 100;
        protected readonly SearchController _controller;

        public abstract IEnumerable<Move> Moves { get; }

        public abstract bool IsTerminal { get; }

        public abstract Player CurentPlayer { get; }

        public abstract int Evaluate();

        public abstract int PosKey { get; }

        private int NegaMax(int depth, int alpha, int beta, SearchController controller)
        {
            controller.IncrementNodes();

            if (depth <= 0 || IsTerminal)
            {
                return Evaluate();
            }

            var first = true;
            var score = NEGINF;
            var oldAlpha = alpha;
            var bestMove = Move.Empty;

            GenerateMoves();

            var pvMove = _controller.PvTable.Probe(PosKey);

            if (pvMove != Move.Empty)
            {
                for (int moveInd = _moveBoundries[_ply]; moveInd < _moveBoundries[_ply + 1]; moveInd++)
                {
                    var move = _moves[moveInd];

                    if (move.Equals(pvMove))
                    {
                        _moveScores[moveInd] = 1000000;
                        break;
                    }
                }
            }

            for (int moveInd = _moveBoundries[_ply]; moveInd < _moveBoundries[_ply + 1]; moveInd++)
            {
                PickNextMove(moveInd);

                var move = _moves[moveInd];

                var hash = PosKey;
                MakeMoveInternal(move);

                score = -NegaMax(depth - 1, -beta, -alpha, controller);

                UndoMoveInternal(move);

                if (hash != PosKey)
                {
                    throw new Exception("Position key error!");
                }

                if (controller.ShouldStop)
                {
                    return 0;
                }

                if (score > alpha)
                {
                    if (score >= beta)
                    {
                        if (first)
                        {
                            controller.FailHighFirst++;
                        }

                        controller.FailHigh++;

                        return beta;
                    }

                    alpha = score;

                    bestMove = move;
                }

                first = false;
            }

            if (alpha != oldAlpha)
            {
                controller.PvTable.Store(PosKey, bestMove);
            }

            return alpha;
        }

        private void UndoMoveInternal(Move move)
        {
            UndoMove(move);

            _ply--;
        }

        private void MakeMoveInternal(Move move)
        {
            _ply++;

            MakeMove(move);
        }

        private void PickNextMove(int moveNum)
        {
            int index = 0;
            int bestScore = -1;
            int bestNum = moveNum;

            for (index = moveNum; index < _moveBoundries[_ply + 1]; index++)
            {
                if (_moveScores[index] > bestScore)
                {
                    bestScore = _moveScores[index];
                    bestNum = index;
                }
            }

            if (bestNum != moveNum)
            {
                var tmp = _moveScores[moveNum];
                _moveScores[moveNum] = _moveScores[bestNum];
                _moveScores[bestNum] = tmp;

                var tmp2 = _moves[moveNum];
                _moves[moveNum] = _moves[bestNum];
                _moves[bestNum] = tmp2;
            }
        }

        private void GenerateMoves()
        {
            var moveIndex = _moveBoundries[_ply];

            var moves = Moves.ToList();
            var score = moves.Count;

            foreach (var move in Moves)
            {
                _moves[moveIndex] = move;
                _moveScores[moveIndex] = score;

                moveIndex++;
                score--;
            }

            _moveBoundries[_ply + 1] = moveIndex;
        }

        private Move[] _moves;
        private int[] _moveScores;
        private int[] _moveBoundries;
        private int _ply;

        public GameStateBase(int movesPerPosition, int maxDepth)
        {
            _controller = new SearchController(maxDepth, TIME_LIMIT, PV_SIZE);

            _moves = new MoveBase[movesPerPosition * maxDepth];
            _moveScores = new int[movesPerPosition * maxDepth];
            _moveBoundries = new int[maxDepth + 1];
        }

        public abstract void UndoMove(Move move);

        public abstract void MakeMove(Move move);

        protected Move SearchPosition()
        {
            var bestMove = Move.Empty;
            var bestScore = NEGINF;
            var curDepth = 1;
            var lastDepth = 0;
            _controller.Reset();

            for (; curDepth <= _controller.MaxDepth; curDepth++)
            {
                bestScore = NegaMax(curDepth, NEGINF, POSINF, _controller);

                if (_controller.ShouldStop)
                {
                    break;
                }

                lastDepth = curDepth;

                bestMove = _controller.PvTable.Probe(PosKey);
            }         

            Console.WriteLine("D:{0}, Best:{1}, Nodes:{2}, Ordering:{3:P2}", curDepth, bestMove, _controller.NodesCount, (double)_controller.FailHighFirst / _controller.FailHigh);

            return bestMove;
        }

        protected Player NextPlayer(Player player)
        {
            return player == Player.One ? Player.Two : Player.One;
        }

        public enum Player
        {
            One = 101010101,
            Two = 202020202
        }

        public class Move
        {
            public static readonly Move Empty = new Move();
        }
    }
}
