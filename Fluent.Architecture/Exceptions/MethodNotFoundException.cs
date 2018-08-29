namespace Fluent.Architecture.Exceptions
{
    public class MethodNotFoundException : NotFoundException
    {
        public MethodNotFoundException(string message) : base(message)
        {
        }
    }
}
