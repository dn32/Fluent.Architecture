namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class NullValueFluentValidationException : FluentValidationException
    {
        public NullValueFluentValidationException(string message) : base(message) { }
    }
}