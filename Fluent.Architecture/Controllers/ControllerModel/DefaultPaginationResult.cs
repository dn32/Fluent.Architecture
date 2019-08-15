// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------


// ReSharper disable CommentTypo

using Fluent.Architecture.Entities;

namespace Fluent.Architecture.Controllers
{
    public class DefaultPaginationResult : DefaultResult
    {
        public FluentPagination Pagination { get; set; }

        public DefaultPaginationResult(object data, FluentPagination pagination) : base(data)
        {
            Pagination = pagination;
        }
    }
}