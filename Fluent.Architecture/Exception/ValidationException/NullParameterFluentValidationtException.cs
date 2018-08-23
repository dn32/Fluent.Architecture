namespace Fluent.Architecture.Exception.ValidationException
{
    public class NullParameterFluentValidationException : FluentValidationException
    {
        public NullParameterFluentValidationException(string parameter) : base($"The parameter {parameter} can not be null.") { }
    }
}