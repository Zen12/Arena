namespace Editor
{
    public class DummyRandom : IRandom
    {
        public int ReturnValue;
        public int GetRandom(in int start, in int end)
        {
            return ReturnValue;
        }
    }
}