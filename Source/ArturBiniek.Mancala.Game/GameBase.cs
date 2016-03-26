using System;
using System.Collections.Generic;
using System.Linq;

namespace ArturBiniek.Mancala.Game
{
    public abstract class GameStateBase
    {
        protected const int MAX_DEPTH = 64;
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

        public GameStateBase()
        {
            _controller = new SearchController(MAX_DEPTH, TIME_LIMIT, PV_SIZE);
        }

        public abstract void UndoMove(Move move);

        public abstract void MakeMove(Move move);

        protected Move SearchPosition()
        {
            var bestMove = Move.Empty;
            var bestScore = NEGINF;
            var curDepth = 1;
            _controller.PvLine = Enumerable.Empty<Move>();

            for (; curDepth <= _controller.MaxDepth; curDepth++)
            {
                bestScore = NegaMax(curDepth, NEGINF, POSINF, _controller);

                if (_controller.ShouldStop)
                {
                    break;
                }

                bestMove = _controller.PvTable.Probe(PosKey);

                var posKey = PosKey;
                _controller.PvLine = GetPvLine(_controller, curDepth);

                if (posKey != PosKey) throw new Exception("Oj oj oj");

                if (_controller.PvLine.Count() < 1) throw new Exception("Pv Line Empty!!!!");
            }



            Console.WriteLine("D:{0}, Best:{1}, Nodes:{2}, Ordering:{3:P2}", curDepth, string.Join("->", _controller.PvLine), _controller.NodesCount, (double)_controller.FailHighFirst / _controller.FailHigh);

            return bestMove;
        }

        public IEnumerable<GameStateBase.Move> GetPvLine(SearchController controller, int depth)
        {
            var res = new List<GameStateBase.Move>(depth);
            var move = controller.PvTable.Probe(PosKey);
            var cnt = 0;

            while (move != GameStateBase.Move.Empty && cnt < depth)
            {
                var exists = Moves.FirstOrDefault(m => m.Equals(move)) != null;

                if (exists)
                {
                    MakeMove(move);

                    res.Add(move);
                    cnt++;
                }
                else
                {
                    break;
                }

                move = controller.PvTable.Probe(PosKey);
            }

            for (int i = res.Count - 1; i >= 0; i--)
                UndoMove(res[i]);

            return res;
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
