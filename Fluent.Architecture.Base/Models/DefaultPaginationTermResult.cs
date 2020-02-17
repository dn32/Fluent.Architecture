namespace Fluent.Architecture.Core.Models
{
    [Attributes.FluentDoc]
    public class DefaultPaginationTermResult<T> : DefaultPaginationResult<T>
    {
        public string Term { get; }

        public DefaultPaginationTermResult() { }

        public DefaultPaginationTermResult(T data, FluentPagination pagination, string term) : base(data, pagination)
        {
            Term = term;
        }
    }
}