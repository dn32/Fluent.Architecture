namespace Fluent.Architecture.Core.Models
{
    [Attributes.FluentDoc]
    public class DefaultPaginationResult<T> : DefaultResult<T>
    {
        public FluentPagination Pagination { get; set; }

        public DefaultPaginationResult() : base() { }

        public DefaultPaginationResult(T data, FluentPagination pagination) : base(data)
        {
            Pagination = pagination;
        }
    }
}