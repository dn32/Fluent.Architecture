// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------
// ReSharper disable CommentTypo

namespace Fluent.Architecture.Core.Models
{
    [Attributes.FluentDoc]
    public class DefaultPaginationResult<T> : DefaultResult<T>
    {
        public FluentPagination Pagination { get; set; }

        public DefaultPaginationResult() : base() { }

        public DefaultPaginationResult(object data, FluentPagination pagination) : base(data)
        {
            Pagination = pagination;
        }
    }
}