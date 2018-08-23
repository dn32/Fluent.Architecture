namespace Fluent.Architecture.Exception.ValidationException
{
    public class FluentValidationException : System.Exception
    {
        public FluentValidationException(string message) : base(message) { }
    }
}