using Fluent.Architecture.Core.Attributes;

namespace Fluent.Architecture.Core.Models
{
    [FluentDoc]
    public class DefaultResult<T>
    {
        public T Data { get; set; }

        public DefaultResult() 
        {
            Data = default;
        }

        public DefaultResult(T data)
        {
            Data = data;
        }
    }
}
