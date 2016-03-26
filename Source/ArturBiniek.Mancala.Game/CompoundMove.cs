namespace ArturBiniek.Mancala.Game
{
    public class CompoundMove : MoveBase
    {
        public readonly MoveBase Kid;

        public CompoundMove(int index, int posKey, MoveBase kid) : base(index, posKey)
        {
            Kid = kid;
        }

        public override string ToString()
        {
            return string.Format("R{0}({1})", BucketIndex, Kid);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode()
                ^ _savedHash.GetHashCode()
                ^ BucketIndex.GetHashCode()
                ^ Kid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as CompoundMove;

            if (other == null) return false;

            var res = false;

            if (this.GetHashCode() == other.GetHashCode())
            {
                res = _savedHash == other._savedHash
                     && BucketIndex == other.BucketIndex
                     && GetType() == other.GetType()
                     && Kid.Equals(other.Kid);
            }

            return res;
        }
    }
}
