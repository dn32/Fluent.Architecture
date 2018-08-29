// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    using System;

    /// <inheritdoc />
    public class FluentParameterValidationException : FluentValidationException
    {
        public FluentParameterValidationException(string parameter, string message) : base(message)
        {
            this.Parameter = parameter;
        }

        public string Parameter { get; set; }
        
        public FluentParameterValidationException(string message, System.Exception innerException) : base(message)
        {
            throw new NotImplementedException();
        }

        public FluentParameterValidationException() : base(string.Empty)
        {
            throw new NotImplementedException();
        }

        public FluentParameterValidationException(string message) : base(message)
        {
            throw new NotImplementedException();
        }
    }
}