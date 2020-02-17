// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------


// ReSharper disable CommentTypo

namespace Fluente.Arquitetura.Nucleo.Models
{
    [Atributos.FluenteDoc]
    public class DefaultTermResult<T> : DefaultResult<T>
    {
        public string Term { get; }

        public DefaultTermResult() { }

        public DefaultTermResult(T data, string term) : base(data)
        {
            Term = term;
        }
    }
}