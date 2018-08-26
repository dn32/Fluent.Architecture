// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exception.ValidationException
{
    public class PropertyNotNullFluentValidationException : FluentValidationException
    {
        public PropertyNotNullFluentValidationException(string propertyName) : base($"The {propertyName} property should not have a value for this operation.") { }
    }
}