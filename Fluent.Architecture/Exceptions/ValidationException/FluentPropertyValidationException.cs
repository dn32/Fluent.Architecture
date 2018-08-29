// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class FluentPropertyValidationException : FluentValidationException
    {
        public string Property { get; set; }

        public FluentPropertyValidationException(string property, string message) : base(message)
        {
            this.Property = property;
        }
    }
}