// ReSharper disable CommentTypo

using System;

namespace Fluent.Architecture.Exceptions.ValidationException
{
    /// <inheritdoc />
    public class FluentValidationException : Exception
    {
        public bool ValidationError => true;

        public FluentValidationException(string message): base(message)
        {
        }
    }
}