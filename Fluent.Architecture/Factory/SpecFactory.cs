using dn32.infra.Exceptions;
using dn32.infra.Services;
using dn32.infra.Specifications;
using System;

namespace dn32.infra.Factory
{
    public static class SpecFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam Nome="T">Tipo de serviço.</typeparam>
        /// <param Nome="service"></param>
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
