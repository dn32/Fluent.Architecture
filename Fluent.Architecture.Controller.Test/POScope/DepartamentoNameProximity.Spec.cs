//using Fluent.Architecture.EntityFramework;
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
//        DescricaoTerm = descricaoTerm;
//        return this;
//    }

//    public override IQueryable<Departamento> Where(IQueryable<Departamento> query)
//    {
//        return query.Where(x => EfContext.jaro_winkler_similarity(ColumnName, DescricaoTerm) > 50);
//    }

//    public override IOrderedQueryable<Departamento> Order(IQueryable<Departamento> query)
//    {
//        return query.OrderBy(x => EfContext.jaro_winkler_similarity(ColumnName, DescricaoTerm));
//    }
//}
