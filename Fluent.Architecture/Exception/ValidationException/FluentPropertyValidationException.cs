namespace Fluent.Architecture.Exception.ValidationException
{
    public class FluentPropertyValidationException : FluentValidationException
    {
        public string Property { get; set; }

        public FluentPropertyValidationException(string property, string message) : base(message)
        {
            Property = property;
        }
    }
}