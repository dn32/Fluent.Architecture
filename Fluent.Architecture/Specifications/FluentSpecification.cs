// ReSharper disable CommentTypo

using System;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Model;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Specifications
{
    /// <summary>
    /// Especificação base para todas as especificações do sistema.
    /// </summary>
    /// <typeparam name="TE">Tipo de entidade da especificação.</typeparam>
    public abstract class FluentSpecification<TE> : BaseSpecification<TE>, IFluentSpecification where TE : BaseEntity
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

        /// <inheritdoc />
        protected FluentSpecification(TransactionalService service) : base(service) { }

        /// <inheritdoc />
        protected FluentSpecification(FluentController<TE> controller) : base(controller) { }

        internal IOrderedQueryable<TE> ToIQueryable(IQueryable<TE> query)
        {
            return Order(Where(query));
        }
    }
}