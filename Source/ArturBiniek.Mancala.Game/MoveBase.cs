namespace ArturBiniek.Mancala.Game
{
    public abstract class MoveBase : GameStateBase.Move
    {
        public readonly int StartIndex;

        public MoveBase(int startIndex)
        {
            StartIndex = startIndex;
        }

        public override string ToString()
        {
            return string.Format("{1} Move: {0}", StartIndex, GetType().Name);
        }
    }
}
