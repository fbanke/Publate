namespace Infrastructure.LinkedIn
{
    public class ContentState
    {
        public enum LifecycleState
        {
            Published,
            Draft
        }

        public LifecycleState State { get; }

        public ContentState(LifecycleState state)
        {
            State = state;
        }

        public override string ToString()
        {
            return State.ToString().ToUpper();
        }
    }
}