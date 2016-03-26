using System;
using System.Collections.Generic;

namespace ArturBiniek.Mancala.Game
{
    public abstract class GameStateBase
    {
        protected const int NEGINF = int.MinValue + 100;
        protected const int POSINF = int.MaxValue - 100;

        public abstract IEnumerable<Move> Moves { get; }

        public abstract bool IsTerminal { get; }

        public abstract Player CurentPlayer { get; }

        public abstract int Evaluate();

        public abstract int PosKey { get; }

        private int NegaMax(int depth, int alpha, int beta, SearchController controller)
        {
            controller.IncrementNodes();

            if (depth == 0 || IsTerminal)
            {
                return Evaluate();
            }

            var first = true;
            var score = NEGINF;
            var oldAlpha = alpha;
            var bestMove = Move.Empty;

            foreach (var move in Moves)
            {
                MakeMove(move);

                score = -NegaMax(depth - 1, -beta, -alpha, controller);

                UndoMove(move);

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

                    // UPDATE HISTORY
                }

                first = false;
            }

            if (alpha != oldAlpha)
            {
                controller.PvTable.Store(PosKey, bestMove);
            }

            return alpha;
        }

        public abstract void UndoMove(Move move);

        public abstract void MakeMove(Move move);

        protected Move SearchPosition(SearchController controller)
        {
            var bestMove = Move.Empty;
            var bestScore = NEGINF;
            var curDepth = 1;

            for (; curDepth <= controller.MaxDepth; curDepth++)
            {
                bestScore = NegaMax(curDepth, NEGINF, POSINF, controller);

                if (controller.ShouldStop)
                {
                    break;
                }

                bestMove = controller.PvTable.Probe(PosKey);
            }

            Console.WriteLine("D:{0}, Best:{1}, Nodes:{2}, Ordering:{3:P2}", curDepth, bestMove, controller.NodesCount, (double)controller.FailHighFirst / controller.FailHigh);

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
