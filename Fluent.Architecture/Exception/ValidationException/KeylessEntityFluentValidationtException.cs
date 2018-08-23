using System;

namespace Fluent.Architecture.Exception.ValidationException
{
    public class KeylessEntityFluentValidationtException : FluentValidationtException
    {
        public KeylessEntityFluentValidationtException(Type entityType) : base($"The {entityType.Name} entity must have a key attribute.") { }
    }
}