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
    public class DefaultPaginationTermResult<T> : DefaultPaginationResult<T>
    {
        public string Term { get; }

        public DefaultPaginationTermResult(object data, FluentPagination pagination, string term) : base(data, pagination)
        {
            Term = term;
        }
    }

    public class DefaultTermResult<T> : DefaultResult<T>
    {
        public string Term { get; }

        public DefaultTermResult(object data, string term) : base(data)
        {
            Term = term;
        }
    }
}