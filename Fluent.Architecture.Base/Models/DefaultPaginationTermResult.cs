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
    public class DefaultPaginationTermResult<T> : DefaultPaginationResult<T>
    {
        public string Term { get; }

        public DefaultPaginationTermResult(object data, FluentPagination pagination, string term) : base(data, pagination)
        {
            Term = term;
        }
    }
}