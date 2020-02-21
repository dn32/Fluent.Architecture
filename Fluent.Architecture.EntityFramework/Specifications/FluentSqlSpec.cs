using dn32.infra.Interfaces;
using dn32.infra.Nucleo.Models;
using dn32.infra.Nucleo.Specifications;
using dn32.infra.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using dn32.infra.dados;
using dn32.infra.extensoes;

#if NETCOREAPP3_1

#else
#endif

namespace dn32.infra.EntityFramework.Specifications
{
    public class DnSqlSpec<TE> : DnSpecification<TE> where TE : DnEntidade
    {
        private string Sql { get; set; }

        private object[] Parameters { get; set; }

        public DnSqlSpec<TE> SetParameter(string sql, params object[] parameters)
        {
            Sql = sql;
            Parameters = parameters;
            return this;
        }

        public override IQueryable<TE> Where(IQueryable<TE> query)
        {
            IgnoreOrder = true;

#if NETCOREAPP3_1
            var dbSet = query.DnCast<DbSet<TE>>();
            return dbSet.FromSqlRaw(Sql, Parameters);
#else
            return query.FromSql(Sql, Parameters);
#endif
        }

        public override IOrderedQueryable<TE> Order(IQueryable<TE> query) => throw new NotImplementedException();
    }
}
