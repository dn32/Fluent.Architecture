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
    public class DefaultPaginationTermResult : DefaultPaginationResult
    {
        public string Term { get; set; }

        public DefaultPaginationTermResult(object data, FluentPagination pagination, string term) : base(data, pagination)
        {
            Term = term;
        }
    }
}