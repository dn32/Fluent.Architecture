namespace Fluent.Architecture.Exception.ValidationException
{
    public class NullParameterFluentValidationtException : FluentValidationtException
    {
        public NullParameterFluentValidationtException(string parameter) : base($"The parameter {parameter} can not be null.") { }
    }
}