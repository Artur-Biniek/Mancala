using System;

namespace ArturBiniek.Mancala.Game
{
    public class SearchController
    {
        private readonly DateTime _deadline;

        public readonly int MaxDepth;

        public readonly PvTable PvTable;

        private int _nodesCount;

        public int NodesCount { get { return _nodesCount; } }

        public int FailHigh;

        public int FailHighFirst;


        public bool ShouldStop
        {
            get; private set;
        }

        public SearchController(int maxDepth, int timeLimitInMs, int pvSize)
        {
            MaxDepth = maxDepth;
            _deadline = DateTime.Now.AddMilliseconds(timeLimitInMs);
            PvTable = new PvTable(pvSize);
        }

        public void IncrementNodes()
        {
            if ((NodesCount & 2047) == 0)
            {
                ShouldStop = DateTime.Now > _deadline;
            }

            _nodesCount++;
        }
    }
}
