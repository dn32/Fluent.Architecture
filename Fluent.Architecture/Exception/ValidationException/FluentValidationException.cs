// ReSharper disable CommentTypo
namespace Fluent.Architecture.Exception.ValidationException
{
    public class FluentValidationException : System.Exception
    {
        public bool ValidationError => true;

        public FluentValidationException(string message) : base(message) { }
    }
}