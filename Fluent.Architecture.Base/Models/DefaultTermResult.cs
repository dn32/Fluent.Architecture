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
    public class DefaultTermResult<T> : DefaultResult<T>
    {
        public string Term { get; }

        public DefaultTermResult() { }

        public DefaultTermResult(object data, string term) : base(data)
        {
            Term = term;
        }
    }
}