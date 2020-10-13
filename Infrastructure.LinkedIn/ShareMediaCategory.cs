namespace Infrastructure.LinkedIn
{
    public class MediaType
    {
        public enum Type
        {
            None,
            
        }

        public Type Category { get; }

        public MediaType(Type type)
        {
            Category = type;
        }

        public override string ToString()
        {
            return Category.ToString().ToUpper();
        }
    }
}