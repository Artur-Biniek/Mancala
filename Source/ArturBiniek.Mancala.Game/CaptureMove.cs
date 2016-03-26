namespace ArturBiniek.Mancala.Game
{
    public class CaptureMove : MoveBase
    {
        public CaptureMove(int index, int posKey) : base(index, posKey)
        {

        }

        public override string ToString()
        {
            return string.Format("C{0}", BucketIndex);
        }
    }
}
