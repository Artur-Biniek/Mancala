using System;

namespace ArturBiniek.Mancala.Game
{
    public abstract class MoveBase : GameStateBase.Move
    {
        public readonly int BucketIndex;
        private int[] _savedBuckets;
        private GameStateBase.Player _savedPlayer;
        private int _savedHash;

        public MoveBase(int bucketIndex)
        {
            BucketIndex = bucketIndex;
        }

        public override string ToString()
        {
            return string.Format("{1} Move: {0}", BucketIndex, GetType().Name);
        }

        public void StoreHistory(int[] buckets, GameStateBase.Player player, int hash)
        {
            _savedBuckets = new int[buckets.Length];
            _savedPlayer = player;
            _savedHash = hash;

            Array.Copy(buckets, _savedBuckets, buckets.Length);
        }

        public void RetoreHistory(ref int[] buckets, ref GameStateBase.Player player, ref int hash)
        {
            buckets = _savedBuckets;
            player = _savedPlayer;
            hash = _savedHash;
        }
    }
}
