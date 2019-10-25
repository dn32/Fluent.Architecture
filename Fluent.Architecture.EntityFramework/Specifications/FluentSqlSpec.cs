using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Core.Specifications;
using Fluent.Architecture.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

#if NETCOREAPP3_0
using Fluent.Architecture.Extensions;
#else
#endif

namespace Fluent.Architecture.EntityFramework.Specifications
{
    public class FluentSqlSpec<TE> : FluentSpecification<TE> where TE : FluentEntity
    {
        private string Sql { get; set; }

        private object[] Parameters { get; set; }

        public FluentSqlSpec<TE> SetParameter(string sql, params object[] parameters)
        {
            Sql = sql;
            Parameters = parameters;
            return this;
        }

        public override IQueryable<TE> Where(IQueryable<TE> query)
        {
            IgnoreOrder = true;

#if NETCOREAPP3_0
            var dbSet = query.FluentCast<DbSet<TE>>();
            return dbSet.FromSqlRaw(Sql, Parameters);
#else
            return query.FromSql(Sql, Parameters);
#endif
        }

        public override IOrderedQueryable<TE> Order(IQueryable<TE> query) => throw new NotImplementedException();
    }
}
