namespace Fluente.Arquitetura.Nucleo.Models
{
    [Atributos.FluenteDoc]
    public class DefaultPaginationResult<T> : DefaultResult<T>
    {
        public FluentePagination Pagination { get; set; }

        public DefaultPaginationResult() : base() { }

        public DefaultPaginationResult(T data, FluentePagination pagination) : base(data)
        {
            Pagination = pagination;
        }
    }
}