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
    public class DefaultPaginationTermResult : DefaultPaginationResult
    {
        public string Term { get; }

        public DefaultPaginationTermResult(object data, FluentPagination pagination, string term) : base(data, pagination)
        {
            Term = term;
        }
    }
}