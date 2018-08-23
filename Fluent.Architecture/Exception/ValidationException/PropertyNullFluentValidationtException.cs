namespace Fluent.Architecture.Exception.ValidationException
{
    public class PropertyNullFluentValidationtException : FluentValidationtException
    {
        public PropertyNullFluentValidationtException(string propertyName) : base($"The property {propertyName} must have a value for this operation.") { }
    }
}