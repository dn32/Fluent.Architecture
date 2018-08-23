namespace Fluent.Architecture.Exception.ValidationException
{
    public class PropertyRequiredFluentValidationtException : FluentValidationtException
    {
        public PropertyRequiredFluentValidationtException(string propertyName) : base($"The property {propertyName} must have a value for this operation.") { }
    }
}