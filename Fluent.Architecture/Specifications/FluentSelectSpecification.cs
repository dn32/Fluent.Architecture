// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Interfaces;
using System;
using System.Linq;

namespace Fluente.Arquitetura.Specifications
{
    /// <summary>
    /// Especificação base para todas as especificações do sistema que tiverem a saida diferente da entrada.
    /// Geralmente o SpecSelect possui em Select nesse caso.
    /// </summary>
    /// <typeparam name="TE">Tipo de entidade da especificação.</typeparam>
    /// <typeparam name="TO">Tipo de objeto de saida da especificação.</typeparam>
    public abstract class FluenteSelectSpecification<TE, TO> : BaseSpecification, IFluenteSpecification<TO> where TE : EntidadeBase
    {
        /// <summary>
        /// A especificação.
        /// </summary>
        /// <param name="query">
        /// A referência à tabela/documento vinda do repositório.
        /// </param>
        /// <returns>
        /// A especificação criada.
        /// </returns>
        public abstract IQueryable<TO> Where(IQueryable<TE> query);

        // Todo2 doc
        public abstract IOrderedQueryable<TO> Order(IQueryable<TO> query);

        // Todo2 doc
        internal IOrderedQueryable<TO> ToIQueryable(IQueryable<TE> query)
        {
            return Order(Where(query));
        }

        public Type FluenteEntityType => typeof(TE);

        public Type FluenteEntityOutType => typeof(TO);
    }
}