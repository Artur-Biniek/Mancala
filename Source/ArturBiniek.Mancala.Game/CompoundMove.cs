namespace ArturBiniek.Mancala.Game
{
    public class CompoundMove : MoveBase
    {
        public readonly MoveBase Kid;

        public CompoundMove(int index, MoveBase kid) : base(index)
        {
            Kid = kid;
        }

        public override string ToString()
        {
            return base.ToString() + " : (" + Kid + ")";
        }
    }
}
