//using Raven.Client.Documents;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;

//namespace Fluent.Architecture
//{
//    public class FluentRepositoryRavenDB<T> : FluentRepository<T> where T : BaseEntity
//    {
//        public TransactionObjects TransactionObjects { get; set; }

//        public override bool Exists(Func<T, bool> specification)
//        {
//            return TransactionObjects.Session.Query<T>().Any(specification);
//        }

//        public virtual List<TOut> FindByTerm<TOut>(Func<TOut, bool> specification, Func<TOut, object> order, FluentPagination pagination)
//        {
//            //Todo - pagination
//            IEnumerable<TOut> query = TransactionObjects.Session.Query<T>().ProjectInto<TOut>();
//            if (specification != null)
//            {
//                query = query.Where(specification);
//            }

//            if (order != null)
//            {
//                return query.OrderBy(order).ToList();
//            }

//            return query.ToList();
//        }

//        public override void SetSession(TransactionObjects transactionObjects)
//        {
//            TransactionObjects = transactionObjects;
//        }

//        public override object StartTransaction()
//        {
//            return TransactionObjects.Session = TransactionObjects.Store.OpenSession();
//        }

//        public override void Commit()
//        {
//            TransactionObjects.Session.SaveChanges();
//            TransactionObjects.Session.Dispose();
//            TransactionObjects.Session = null;
//        }

//        public override void Rollback()
//        {
//            TransactionObjects.Session.Dispose();
//            TransactionObjects.Session = null;
//        }

//        public override int Count(Expression<Func<T, bool>> specification)
//        {
//            return TransactionObjects.Session.Query<T>().Count(specification);
//        }

//        public override List<TOut> FindList<TOut>(Func<TOut, bool> specification, Func<TOut, object> order, FluentPagination pagination)
//        {
//            //Todo - pagination
//            var query = TransactionObjects.Session.Query<T>().ProjectInto<TOut>().Where(specification);

//            if (order != null)
//            {
//                return query.OrderBy(order).ToList();
//            }

//            return query.ToList();
//        }

//        public override List<T> FindAll(Func<T, object> order, FluentPagination pagination)
//        {
//            return FindList((x) => true, order, pagination);
//        }

//        public override void AddSeveral(params T[] entities)
//        {
//            foreach (var entity in entities)
//            {
//                TransactionObjects.Session.Store(entity);
//            }
//        }

//        public override void Add(T entity)
//        {
//            TransactionObjects.Session.Store(entity);
//        }

//        public override T Find(int id)
//        {
//            return TransactionObjects.Session.Load<T>(id);
//        }

//        public override void Update(T entity)
//        {
//            TransactionObjects.Session.Store(entity);
//        }

//        public override void Delete(int id)
//        {
//            TransactionObjects.Session.Delete(id);
//        }
        
//        public virtual void XYZ(T entity)
//        {
//            TransactionObjects.Session.Store(entity);
//        }
//    }
//}