// ReSharper disable CommentTypo

namespace Fluent.Architecture.Model
{
    public class ValidationReturn
    {
        public string Message { get; set; }
        public bool ValidationError { get; internal set; }
    }
}