using Fluent.Architecture.Entities;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Fluent.Architecture.EntityFramework.Oracle.Specifications
{
    public class TermByProximitySpec<TE> : FluentSpecification<TE> where TE : FluentEntity
    {
        private string Term { get; set; }

        private string TableName { get; set; }

        private string ColumnName { get; set; }

        private int Tolerance { get; set; }

        public TermByProximitySpec<TE> AddParameter(string property, string term, int tolerance)
        {
            TableName = typeof(TE).GetTableName();
            ColumnName = typeof(TE).GetProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase))?.GetColumnName() ?? throw new Exception($"Property not found {typeof(TE).Name}.{property}");
            Term = term;
            Tolerance = tolerance == 0 ? 80 : tolerance;
            return this;
        }

        public override IQueryable<TE> Where(IQueryable<TE> query)
        {
            IgnoreOrder = true;
            return query.WhereProximityText(Term, TableName, ColumnName, Tolerance);
        }

        public override IOrderedQueryable<TE> Order(IQueryable<TE> query) => throw new NotImplementedException();
    }
}
