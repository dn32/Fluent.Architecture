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

[assembly: InternalsVisibleTo(@"Fluent.Architecture.EntityFramework.SqlServer, PublicKey=0024000004800000940000000602000000240000525341310004000001000100695b7abf1265ea2f1aa5bcca4155cb0a8a657b2995a7b278b4a108f6d386cd4bf80544b30aa3f4f230e6f880bfd7ed3200580ce949729eafbcf456a61e07571caf703c916bf9c175f79adf693db191e9f01f128076122f37a8ab1902035a809bad424c34edadc97c9f1e4744a1cd3a473912182bedd56d5dc4ceb71696cbbbc1")]
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

        // Todo doc
        public abstract IOrderedQueryable<TE> Order(IQueryable<TE> query);

        internal IOrderedQueryable<TE> ToIQueryable(IQueryable<TE> query)
        {
            return Order(Where(query));
        }
    }
}