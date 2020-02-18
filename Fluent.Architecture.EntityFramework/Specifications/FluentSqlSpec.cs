using Fluente.Arquitetura.Interfaces;
using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Nucleo.Specifications;
using Fluente.Arquitetura.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using dn32.infra.dados;
using dn32.infra.extensoes;

#if NETCOREAPP3_1
using Fluente.Arquitetura.Extensoes;
#else
#endif

namespace Fluente.Arquitetura.EntityFramework.Specifications
{
    public class FluenteSqlSpec<TE> : FluenteSpecification<TE> where TE : FluenteEntidade
    {
        private string Sql { get; set; }

        private object[] Parameters { get; set; }

        public FluenteSqlSpec<TE> SetParameter(string sql, params object[] parameters)
        {
            Sql = sql;
            Parameters = parameters;
            return this;
        }

        public override IQueryable<TE> Where(IQueryable<TE> query)
        {
            IgnoreOrder = true;

#if NETCOREAPP3_1
            var dbSet = query.FluenteCast<DbSet<TE>>();
            return dbSet.FromSqlRaw(Sql, Parameters);
#else
            return query.FromSql(Sql, Parameters);
#endif
        }

        public override IOrderedQueryable<TE> Order(IQueryable<TE> query) => throw new NotImplementedException();
    }
}
