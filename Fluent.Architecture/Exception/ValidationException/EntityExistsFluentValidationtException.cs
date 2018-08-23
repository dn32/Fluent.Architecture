namespace Fluent.Architecture.Exception.ValidationException
{
    public class EntityExistsFluentValidationtException : FluentValidationtException
    {
        public EntityExistsFluentValidationtException(string entityKeys) : base($"An entity with any of these keys already exists in the database: {entityKeys}") { }
    }
}