// ReSharper disable CommentTypo

#if NETCOREAPP2_1

using Microsoft.EntityFrameworkCore;

#else

using System.Data.Entity;

#endif

using System;
using System.Linq;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Interfaces;
using Fluent.Architecture.Model;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Specifications
{
    /// <summary>
    /// Especificação base para todas as especificações do sistema que tiverem a saida diferente da entrada.
    /// Geralmente o SpecSelect possui em Select nesse caso.
    /// </summary>
    /// <typeparam name="TE">Tipo de entidade da especificação.</typeparam>
    /// <typeparam name="TO">Tipo de objeto de saida da especificação.</typeparam>
    public abstract class FluentSelectSpecification<TE, TO> : BaseSpecification<TE>, IFluentSpecification<TO> where TE : BaseEntity
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
        public abstract IQueryable<TO> Where(DbSet<TE> query);

        // Todo doc
        public abstract IOrderedQueryable<TO> Order(IQueryable<TO> query);

        // Todo doc
        internal IOrderedQueryable<TO> ToIQueryable(DbSet<TE> query)
        {
            var queryWhere = Where(query);

#if NETCOREAPP2_1

            if (typeof(TO).IsClass)
            {
                //Todo - Testar esse método
                queryWhere = typeof(EntityFrameworkQueryableExtensions)
                                .GetMethod(nameof(EntityFrameworkQueryableExtensions.AsNoTracking))
                                .MakeGenericMethod(typeof(TO))
                                .Invoke(null, new object[] { queryWhere }) as IQueryable<TO>;
            }

#else
            queryWhere = queryWhere.AsNoTracking() as IQueryable<TO>;
#endif

            return Order(queryWhere);
        }

        /// <inheritdoc />
        protected FluentSelectSpecification(FluentController<TE> controller) : base(controller) { }

        /// <inheritdoc />
        protected FluentSelectSpecification(TransactionalService service) : base(service) { }

        public Type FluentEntityType => typeof(TE);

        public Type FluentEntityOutType => typeof(TO);
    }
}