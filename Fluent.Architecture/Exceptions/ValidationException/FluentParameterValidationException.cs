// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exceptions.ValidationException
{
    /// <inheritdoc />
    public class FluentParameterValidationException : FluentValidationException
    {
        public string Parameter { get; set; }

        public FluentParameterValidationException(string parameter, string message) : base(message)
        {
            this.Parameter = parameter;
        }
    }
}