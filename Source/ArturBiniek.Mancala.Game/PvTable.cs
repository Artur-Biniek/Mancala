using System;

namespace ArturBiniek.Mancala.Game
{
    public class PvTable
    {
        private readonly PvEntry[] _entries;

        public PvTable(int maxEntries)
        {
            if (maxEntries < 1) throw new ArgumentOutOfRangeException();

            _entries = new PvEntry[maxEntries];
        }

        public GameStateBase.Move Probe(int posKey)
        {
            var index = posKey % _entries.Length;
            var entry = _entries[index];

            if (entry != null && entry.PosKey == posKey)
            {
                return entry.Move;
            }

            return GameStateBase.Move.Empty;
        }

        public void Store(int posKey, GameStateBase.Move move)
        {
            var index = posKey % _entries.Length;

            _entries[index] = new PvEntry(posKey, move); 
        }

        private class PvEntry
        {
            public readonly int PosKey;
            public readonly GameStateBase.Move Move;

            public PvEntry(int posKey, GameStateBase.Move move)
            {
                PosKey = posKey;
                Move = move;
            }
        }
    }
}
