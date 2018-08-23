namespace Fluent.Architecture.Exception.ValidationException
{
    public class EntityNotFoundFluentValidationtException : FluentValidationtException
    {
        public EntityNotFoundFluentValidationtException(string entityKeys) : base($"No entity with this key(s) was found in the database: {entityKeys}") { }
    }
}