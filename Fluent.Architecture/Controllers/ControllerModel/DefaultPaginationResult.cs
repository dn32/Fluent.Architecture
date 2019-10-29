// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Core.Models;

// ReSharper disable CommentTypo

namespace Fluent.Architecture.Controllers
{
    public class DefaultPaginationResult<T> : DefaultResult<T>
    {
        public FluentPagination Pagination { get; set; }

        public DefaultPaginationResult(object data, FluentPagination pagination) : base(data)
        {
            Pagination = pagination;
        }
    }
}