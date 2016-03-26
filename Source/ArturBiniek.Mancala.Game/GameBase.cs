using System.Collections.Generic;

namespace ArturBiniek.Mancala.Game
{
    public abstract class GameStateBase
    {
        protected const int NEGINF = int.MinValue + 100;
        protected const int POSINF = int.MaxValue - 100;

        public abstract IEnumerable<Move> Moves { get; }

        protected abstract bool IsTerminal { get; }

        protected abstract Player CurentPlayer { get; }

        protected abstract int Evaluate();

        private int NegaMax(int depth, int alpha, int beta, SearchController controller)
        {
            if (depth == 0 || IsTerminal)
            {
                return Evaluate();
            }

            controller.IncrementNodes();

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
                // STORE PV MOVE
            }

            return alpha;
        }

        public abstract void UndoMove(Move move);

        public abstract void MakeMove(Move move);

        public Move SearchPosition(SearchController controller)
        {
            var bestMove = Move.Empty;
            var bestScore = NEGINF;

            for (var curDepth = 1; curDepth <= controller.MaxDepth; curDepth++)
            {

                // AB

                if (controller.ShouldStop)
                {
                    break;
                }
            }

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
