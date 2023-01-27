namespace Permutation.Editor
{
    public class DummyListener : IPermutationListener
    {
        public PermutationChanges Last;
        public void OnChangeModel(in PermutationChanges changes)
        {
            Last = changes;
        }
    }

    public class DummyTimeline : ITimeline
    {
        public float CurrentTime { get; set; }
    }
}