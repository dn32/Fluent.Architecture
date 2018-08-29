// ReSharper disable CommentTypo

namespace Fluent.Architecture.Exceptions.ValidationException
{
    using System;
    using System.Runtime.Serialization;

    using Exception = System.Exception;

    /// <inheritdoc />
    public class FluentValidationException : Exception
    {
        public bool ValidationError => true;

        public FluentValidationException(string message)
            : base(message)
        {
        }

        public FluentValidationException()
        {
            throw new NotImplementedException();
        }

        public FluentValidationException(string message, System.Exception innerException) : base(message, innerException)
        {
            throw new NotImplementedException();
        }

        protected FluentValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}