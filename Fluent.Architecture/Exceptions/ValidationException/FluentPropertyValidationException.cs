// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    using System;

    public class FluentPropertyValidationException : FluentValidationException
    {
        public string Property { get; set; }

        public FluentPropertyValidationException(string property, string message) : base(message)
        {
            this.Property = property;
        }

        public FluentPropertyValidationException()
            : base(string.Empty)
        {
            throw new NotImplementedException();
        }

        public FluentPropertyValidationException(string message) : base(message)
        {
            throw new NotImplementedException();
        }

        public FluentPropertyValidationException(string message, System.Exception innerException) : base(message)
        {
            throw new NotImplementedException();
        }
    }
}