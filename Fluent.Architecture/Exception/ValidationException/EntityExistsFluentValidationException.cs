namespace Fluent.Architecture.Exception.ValidationException
{
    public class EntityExistsFluentValidationException : FluentValidationException
    {
        public EntityExistsFluentValidationException(string entityKeys) : base($"An entity with any of these keys already exists in the database: {entityKeys}") { }
    }
}