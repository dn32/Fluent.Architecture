// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Model;

[assembly: InternalsVisibleTo(@"Fluent.Architecture.EntityFramework.SqlServer, PublicKey=00240000048000009400000006020000002400005253413100040000010001002d98533364f3b3fbd11e7a3f14cd73d169e1daabd62ba2d1e5bc6a48a9bc709a503960db0e76c190e7a8dcefaed037e539682d6a891b242ddb91a3ab20fbfa0c04fb6304c8903857e1ed75399850fca4037dd2c810749e75770e5d455e950ccb9d06cf6fea5f30b00557a29408ce4c45021c412eca32616f47809bfe2cf404cc")]
namespace Fluent.Architecture.Specifications
{
    /// <summary>
    /// Especificação base para todas as especificações do sistema.
    /// </summary>
    /// <typeparam name="TE">Tipo de entidade da especificação.</typeparam>
    public abstract partial class FluentSpecification<TE> : BaseSpecification, IFluentSpecification where TE : BaseEntity
    {
        public Type FluentEntityType => typeof(TE);

        /// <summary>
        /// A especificação.
        /// </summary>
        /// <param name="query">
        /// A referência à tabela/documento vinda do repositório.
        /// </param>
        /// <returns>
        /// A especificação criada.
        /// </returns>
        public abstract IQueryable<TE> Where(IQueryable<TE> query);

        // Todo2 doc
        public abstract IOrderedQueryable<TE> Order(IQueryable<TE> query);

        internal IOrderedQueryable<TE> ToIQueryable(IQueryable<TE> query)
        {
            return Order(Where(query));
        }
    }
}