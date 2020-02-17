namespace Fluente.Arquitetura.Nucleo.Models
{
    [Atributos.FluenteDoc]
    public class DefaultPaginationTermResult<T> : DefaultPaginationResult<T>
    {
        public string Term { get; }

        public DefaultPaginationTermResult() { }

        public DefaultPaginationTermResult(T data, FluentePagination pagination, string term) : base(data, pagination)
        {
            Term = term;
        }
    }
}