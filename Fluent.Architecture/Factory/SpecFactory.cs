using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using System;

namespace Fluent.Architecture.Factory
{
    public static class SpecFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Tipo de serviço.</typeparam>
        /// <param name="service"></param>
        /// <returns></returns>
        public static T Create<T>(TransactionalService service) where T : BaseSpecification
        {
            if (!(Activator.CreateInstance(typeof(T)) is T ts))
            {
                throw new IncorrectDevelopmentException($"Failed to initialize specification [{typeof(T).Name}] type with specified constructor parameters not found.");
            }

            ts.SetService(service);
            return ts;
        }
    }
}
