namespace Infrastructure.LinkedIn
{
    public class Visibility
    {
        public readonly string JsonName = "com.linkedin.ugc.MemberNetworkVisibility";

        public enum Reach
        {
            Connections,
            Public,
        }

        public Reach State { get; }

        public Visibility(Reach state)
        {
            State = state;
        }
        
        public override string ToString()
        {
            return State.ToString().ToUpper();
        }
    }
}