// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exception.ValidationException
{
    public class FluentParameterValidationException : FluentValidationException
    {
        public string Parameter { get; set; }

        public FluentParameterValidationException(string parameter, string message) : base(message)
        {
            Parameter = parameter;
        }
    }
}