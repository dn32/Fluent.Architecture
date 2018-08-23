namespace Fluent.Architecture.Exception.ValidationException
{
    public class PropertyNotNullFluentValidationtException : FluentValidationtException
    {
        public PropertyNotNullFluentValidationtException(string propertyName) : base($"The {propertyName} property should not have a value for this operation.") { }
    }
}