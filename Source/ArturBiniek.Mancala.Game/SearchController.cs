using System;
using System.Collections.Generic;
using System.Linq;

namespace ArturBiniek.Mancala.Game
{
    public class SearchController
    {
        private DateTime _deadline;

        public int MaxDepth
        {
            get; private set;
        }

        public PvTable PvTable;

        private int _nodesCount;

        public int NodesCount { get { return _nodesCount; } }

        public int FailHigh;

        public int FailHighFirst;
        private int _timeInMs;
        private int _pvSize;

        public bool ShouldStop
        {
            get; private set;
        }

        public SearchController(int maxDepth, int timeLimitInMs, int pvSize)
        {
            MaxDepth = maxDepth;
            _timeInMs = timeLimitInMs;
            _pvSize = pvSize;
            _deadline = DateTime.Now.AddMilliseconds(_timeInMs);
            PvTable = new PvTable(_pvSize);
        }

        public void IncrementNodes()
        {
            if ((NodesCount & 2047) == 0)
            {
                ShouldStop = DateTime.Now > _deadline;
            }

            _nodesCount++;
        }

        public void Reset()
        {
            ShouldStop = false;
            _deadline = DateTime.Now.AddMilliseconds(_timeInMs);
            PvTable = new PvTable(_pvSize);           
        }
    }
}
