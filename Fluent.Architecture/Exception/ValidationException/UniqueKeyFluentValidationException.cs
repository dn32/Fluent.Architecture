namespace Fluent.Architecture.Exception.ValidationException
{
    public class UniqueKeyFluentValidationException : FluentValidationException
    {
        public UniqueKeyFluentValidationException(string propertyName, string value) : base($"A record already exists in the database with the value {value} for the {propertyName}.") { }
    }
}