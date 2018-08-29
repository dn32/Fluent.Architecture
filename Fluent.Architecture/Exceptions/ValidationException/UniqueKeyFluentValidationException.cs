// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    using System;

    public class UniqueKeyFluentValidationException : FluentValidationException
    {
        public UniqueKeyFluentValidationException(string propertyName, string value)
            : base($"A record already exists in the database with the value {value} for the {propertyName}.")
        {
        }

        public UniqueKeyFluentValidationException()
            : base(string.Empty)
        {
            throw new NotImplementedException();
        }

        public UniqueKeyFluentValidationException(string message) : base(message)
        {
            throw new NotImplementedException();
        }

        public UniqueKeyFluentValidationException(string message, System.Exception innerException) : base(message)
        {
            throw new NotImplementedException();
        }
    }
}