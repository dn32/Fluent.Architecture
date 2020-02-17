using Fluente.Arquitetura.Nucleo.Atributos;

namespace Fluente.Arquitetura.Nucleo.Models
{
    [FluenteDoc]
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
