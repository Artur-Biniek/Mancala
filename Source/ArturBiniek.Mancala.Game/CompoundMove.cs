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
    }
}
