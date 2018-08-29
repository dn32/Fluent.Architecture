// ReSharper disable CommentTypo

namespace Fluent.Architecture.Exceptions.ValidationException
{
    using Exception = System.Exception;

    /// <inheritdoc />
    public class FluentValidationException : Exception
    {
        public bool ValidationError => true;

        public FluentValidationException(string message)
            : base(message)
        {
        }
    }
}