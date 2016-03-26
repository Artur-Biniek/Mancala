namespace ArturBiniek.Mancala.Game
{
    public class NormalMove : MoveBase
    {
        public NormalMove(int index, int posKey) : base(index, posKey)
        {

        }


        public override string ToString()
        {
            return string.Format("N{0}", BucketIndex);
        }
    }
}
