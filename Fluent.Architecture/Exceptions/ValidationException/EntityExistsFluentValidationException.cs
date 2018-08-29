// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    using System;

    public class EntityExistsFluentValidationException : FluentValidationException
    {
        public EntityExistsFluentValidationException(string entityKeys)
            : base($"An entity with any of these keys already exists in the database: {entityKeys}")
        {
        }

        public EntityExistsFluentValidationException()
            : base(string.Empty)
        {
            throw new NotImplementedException();
        }

        public EntityExistsFluentValidationException(string message, System.Exception innerException) : base(message)
        {
            throw new NotImplementedException();
        }
    }
}