namespace Fluent.Architecture.Exception.ValidationException
{
    public class EntityNotFoundFluentValidationException : FluentValidationException
    {
        public EntityNotFoundFluentValidationException(string entityKeys) : base($"No entity with this key(s) was found in the database: {entityKeys}") { }
    }
}