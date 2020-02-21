using dn32.infra.extensoes;
using dn32.infra.Specifications;
using System;
using System.Linq;
using dn32.infra.dados;

namespace dn32.infra.EntityFramework.Oracle.Specifications
{
    public class TermByProximitySpec<TE> : DnSpecification<TE> where TE : DnEntidade
    {
        private string Term { get; set; }

        private string TableName { get; set; }

        private string[] Columns { get; set; }

        private int Tolerance { get; set; }

        public TermByProximitySpec<TE> AddParameter(string[] properties, string term, int tolerance)
        {
            TableName = typeof(TE).GetTableName();
            Columns = properties.Select(property => typeof(TE).GetProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase))?.GetColumnName() ?? throw new Exception($"Propriedade not found {typeof(TE).Name}.{property}")).ToArray();
            Term = term;
            Tolerance = tolerance == 0 ? 80 : tolerance;
            return this;
        }

        public override IQueryable<TE> Where(IQueryable<TE> query)
        {
            IgnoreOrder = true;
            return query.WhereProximityText(Term, TableName, Columns, Tolerance);
        }

        public override IOrderedQueryable<TE> Order(IQueryable<TE> query) => throw new NotImplementedException();
    }
}
