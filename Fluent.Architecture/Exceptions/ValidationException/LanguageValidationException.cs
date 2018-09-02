namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class LanguageValidationException : FluentValidationException
    {
        public LanguageValidationException(string message) : base(message)
        {
        }
    }
}