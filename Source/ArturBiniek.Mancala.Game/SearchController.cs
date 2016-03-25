using System;

namespace ArturBiniek.Mancala.Game
{
    public class SearchController
    {
        private readonly DateTime _deadline;

        public readonly int MaxDepth;

        public int NodesCount;

        public int FailHigh;

        public int FailHighFirst;


        public bool ShouldStop
        {
            get { return DateTime.Now > _deadline; }
        }

        public SearchController(int maxDepth, int timeLimitInMs)
        {
            MaxDepth = maxDepth;
            _deadline = DateTime.Now.AddMilliseconds(timeLimitInMs);
        }
    }
}
