//using Fluent.Architecture.Entities;
//using System.Linq;

//namespace Fluent.Architecture.Specifications
//{
//    public class AllSpec<T> : FluentSpecification<T> where T : FluentEntity
//    {
//        public override IQueryable<T> Where(IQueryable<T> query)
//        {
//            return query;
//        }

//        public override IOrderedQueryable<T> Order(IQueryable<T> query)
//        {
//            return query.OrderBy(x => x);
//        }
//    }
//}
