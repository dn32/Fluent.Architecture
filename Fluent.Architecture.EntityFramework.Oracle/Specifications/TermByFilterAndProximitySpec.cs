using dn32.infra.extensoes;
using dn32.infra.Nucleo.Extensoes;
using dn32.infra.Specifications;
using System;
using System.Linq;
using dn32.infra.dados;
using dn32.infra.nucleo.extensoes;

namespace dn32.infra.EntityFramework.Oracle.Specifications
{
    public class TermByFilterAndProximitySpec<T> : DnSpecification<T> where T : DnEntidade
    {
        private string Term { get; set; }

        private string TableName { get; set; }

        private string[] Columns { get; set; }

        private int Tolerance { get; set; }

        public Filtro[] Filters { get; set; }

        public bool IsList { get; set; }

        public TermByFilterAndProximitySpec<T> SetParameter(Filtro[] filters, bool isList, string[] properties, string term, int tolerance)
        {
            Filters = filters;
            IsList = isList;
            TableName = typeof(T).GetTableName();
            Term = term;

            if (properties?.Length > 0 && !string.IsNullOrWhiteSpace(Term))
            {
                Columns = properties.Select(property => typeof(T).GetProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase))?.GetColumnName() ?? throw new Exception($"Propriedade not found '{typeof(T).Name}.{property}'")).ToArray();
            }

            Tolerance = tolerance == 0 ? 80 : tolerance;
            return this;
        }

        public override IQueryable<T> Where(IQueryable<T> query)
        {
            IgnoreOrder = true;
            var expression = Filters.ConverterFiltrosParaExpressao<T>();

            query = query
                     .Where(expression)
                     .GetInclusions(IsList);

            if (Columns?.Length > 0 && !string.IsNullOrWhiteSpace(Term))
            {
                query = query.WhereProximityText(Term, TableName, Columns, Tolerance);
            }

            return query.ProjetarDeFormaDinamica(Service);
        }

        public override IOrderedQueryable<T> Order(IQueryable<T> query) => throw new NotImplementedException();
    }
}
