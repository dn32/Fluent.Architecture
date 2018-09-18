// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    /// <inheritdoc />
    public class NullFluentValidationException : NullValueFluentValidationException
    {
        public NullFluentValidationException(string parameter)
            : base($"The parameter {parameter} can not be null.")
        {
        }
    }
}