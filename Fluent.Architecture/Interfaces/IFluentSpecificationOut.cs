using System;

namespace Fluent.Architecture.Interfaces
{
    public interface IFluentSpecificationOut
    {
        Type FluentEntityType { get; }
        Type FluentEntityOutType { get; }
    }

    public interface IFluentSpecification<TO> : IFluentSpecificationOut
    {
    }
}