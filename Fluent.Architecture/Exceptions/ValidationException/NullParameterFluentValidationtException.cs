// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class NullParameterFluentValidationException : FluentValidationException
    {
        public NullParameterFluentValidationException(string parameter) : base($"The parameter {parameter} can not be null.") { }
    }
}