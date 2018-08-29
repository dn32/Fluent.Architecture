// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class PropertyRequiredFluentValidationException : FluentValidationException
    {
        public PropertyRequiredFluentValidationException(string propertyName) : base($"The property {propertyName} must have a value for this operation.") { }
    }
}