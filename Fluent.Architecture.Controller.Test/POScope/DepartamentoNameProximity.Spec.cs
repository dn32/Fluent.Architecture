//using Fluent.Architecture.EntityFramework.Oracle;
//using Fluent.Architecture.Extensions;
//using Fluent.Architecture.Specifications;
//using System.Linq;

//public class DepartamentoNameProximity : FluentSpecification<Departamento>
//{
//    private string DescricaoTerm { get; set; }

//    private string ColumnName { get; set; }

//    public DepartamentoNameProximity AddParameter(string descricaoTerm)
//    {
//        ColumnName = typeof(Departamento).GetProperty(nameof(Departamento.Descricao)).GetColumnName();
//        DescricaoTerm = descricaoTerm.ToUpper();
//        return this;
//    }

//    public override IQueryable<Departamento> Where(IQueryable<Departamento> query)
//    {
//        return query;
//    }

//    public override IOrderedQueryable<Departamento> Order(IQueryable<Departamento> query)
//    {
//        return query.OrderByDescending(x => SimilarityComparator.Compare(x.Descricao, DescricaoTerm));
//    }
//}
