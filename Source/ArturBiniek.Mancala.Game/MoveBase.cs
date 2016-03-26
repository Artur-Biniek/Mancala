using System;

namespace ArturBiniek.Mancala.Game
{
    public abstract class MoveBase : GameStateBase.Move
    {
        public readonly int BucketIndex;
        private int[] _savedBuckets;
        private GameStateBase.Player _savedPlayer;
        private int _savedHash;

        public MoveBase(int bucketIndex, int posKey)
        {
            BucketIndex = bucketIndex;
            _savedHash = posKey;
        }

        public override string ToString()
        {
            return string.Format("{1} Move: {0}", BucketIndex, GetType().Name);
        }

        public void StoreHistory(int[] buckets, GameStateBase.Player player)
        {
            _savedBuckets = new int[buckets.Length];
            _savedPlayer = player;

            Array.Copy(buckets, _savedBuckets, buckets.Length);
        }

        public void RetoreHistory(ref int[] buckets, ref GameStateBase.Player player, ref int hash)
        {
            buckets = _savedBuckets;
            player = _savedPlayer;
            hash = _savedHash;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode()              
                ^ _savedHash.GetHashCode()
                ^ BucketIndex.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (MoveBase)obj;

            if (other == null) return false;

            var res = false; 

            if (this.GetHashCode() == other.GetHashCode())
            {
               res = _savedHash == other._savedHash
                    && BucketIndex == other.BucketIndex
                    && GetType() == other.GetType();
            }

            return res;
        }
    }
}
