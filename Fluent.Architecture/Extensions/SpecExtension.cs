using Fluent.Architecture.Entities;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Interfaces;
using System.Collections.Generic;

namespace Fluent.Architecture.Extensions
{
    public static class SpecExtension
    {
        public static bool Exists(this ISpec spec)
        {
            return (bool)spec.Execute(nameof(Exists));
        }

        public static object List(this ISpec spec, FluentPagination pagination)
        {
            return spec.Execute(nameof(List), new object[] { pagination });
        }

        public static object List(this ISpec spec)
        {
            return spec.Execute(nameof(List), new object[] { });
        }

        public static object FirstOrDefault(this ISpec spec)
        {
            return spec.Execute(nameof(FirstOrDefault));
        }

        public static object Execute(this ISpec spec, string method, params object[] parameters)
        {
            return spec.CallSelectOrNoSelectMethod(method, parameters);
        }

        public static object CallSelectOrNoSelectMethod(this ISpec spec, string methodName, params object[] parameters)
        {
            var paramList = new List<object> { spec };
            paramList.AddRange(parameters);
            parameters = paramList.ToArray();

            if (spec != null && spec.Service == null)
            {
                throw new IncorrectDevelopmentException($"The past spec does not have a valid service. See at the time the spec is created if a service has been passed in the spec creator.");
            }

            if (spec is IFluentSpecificationOut spec2)
            {
                var service = spec2.FluentEntityType.GetServiceInstanceByEntity(spec2.Service.SessionRequest);
                var method = service.GetType().GetMethod($"{methodName}Select");
                if (method == null)
                {
                    throw new IncorrectDevelopmentException($"Method not found: {methodName}Select");
                }

                return method.MakeGenericMethod(spec2.FluentEntityOutType).Invoke(service, parameters);
            }

            if (spec is IFluentSpecification spec3)
            {
                var service = spec3.FluentEntityType.GetServiceInstanceByEntity(spec3.Service.SessionRequest);
                var method = service.GetType().GetMethodWithoutAmbiguity(methodName, parameters);
                if (method == null)
                {
                    throw new IncorrectDevelopmentException($"Method not found: {methodName}");
                }

                return method.Invoke(service, parameters);
            }

            throw new IncorrectDevelopmentException("The specification is of a different type than expected");
        }

        //public static T Add<T>(this TransactionalService service, T entity) where T : BaseEntity
        //{
        //    return service.Add(entity);
        //}

        //public static bool Exists(this TransactionalService service, IFluentSpecification spec)
        //{
        //    return spec.Exists();
        //}

        //public static bool Exists<TO>(this TransactionalService service, IFluentSpecification<TO> spec)
        //{
        //    return spec.Exists();
        //}

        //public static void AddRange<T>(this TransactionalService service, params T[] entities) where T : BaseEntity
        //{
        //    return service.AddRange(entities);
        //}

        //public static int Count<TO>(this TransactionalService service, IFluentSpecification<TO> spec)
        //{
        //    return service.Count(spec);
        //}

        ////public static int Count(this TransactionalService service)
        ////{
        ////    return service.Count(spec);
        ////}

        //public static int Count(this TransactionalService service, IFluentSpecification spec)
        //{
        //    return spec.Count();
        //}


        //public static T Find<T>(this TransactionalService service, T entity) where T : BaseEntity
        //{
        //    return service.Find(entity);
        //}

        //public static TO FirstOrDefault<TO>(this TransactionalService service, IFluentSpecification<TO> spec)
        //{
        //    return service.FirstOrDefault(entity);
        //}

        //public static T FirstOrDefault<T>(this TransactionalService service, IFluentSpecification spec) where T : BaseEntity
        //{
        //    return service.FirstOrDefault(entity);
        //}

        //public static T FirstOrDefault<T>(this TransactionalService service) where T : BaseEntity
        //{
        //    return service.FirstOrDefault(entity);
        //}

        //public static List<TO> List<TO>(this TransactionalService service, IFluentSpecification<TO> spec, FluentPagination pagination = null)
        //{
        //    return service.List(spec, pagination);
        //}

        //public static List<T> List<T>(this TransactionalService service, IFluentSpecification spec, FluentPagination pagination = null) where T : BaseEntity
        //{
        //    return service.List(spec, pagination);
        //}

        //public static T Remove<T>(this TransactionalService service, T entity) where T : BaseEntity
        //{
        //    return service.Remove(entity);
        //}

        //public static void RemoveRange(this TransactionalService service, IFluentSpecification spec)
        //{
        //    service.RemoveRange(spec);
        //}

        //public static void RemoveRange<T>(this TransactionalService service, params T[] entities) where T : BaseEntity
        //{
        //    service.AddRange(entities);
        //}

        //public static T Update<T>(this TransactionalService service, T entity) where T : BaseEntity
        //{
        //    return service.Update(entity);
        //}

        #region PRIVATES

        //internal static FluentSpecification<TE> GetSpec<TE>(this IFluentSpecification spec) where TE : BaseEntity
        //{
        //    return spec as FluentSpecification<TE>;
        //}

        //private static FluentService<TE> GetService<TE>(UserSessionRequest sessionRequest) where TE : BaseEntity
        //{
        //    return typeof(TE).GetServiceInstanceByEntity(sessionRequest) as FluentService<TE>;
        //}

        #endregion
    }
}
