using System;
using Fluent.Architecture.Exceptions;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;

namespace Fluent.Architecture.Factory
{
    public static class SpecFactory
    {
        public static Ts Create<Ts>(TransactionalService service) where Ts : BaseSpecification
        {
            if (!(Activator.CreateInstance(typeof(Ts)) is Ts ts))
            {
                throw new IncorrectDevelopmentException($"Failed to initialize specification [{typeof(Ts).Name}] type with specified constructor parameters not found.");
            }

            ts.SetService(service);
            return ts;
        }
    }
}
