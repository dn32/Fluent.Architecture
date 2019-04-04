// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Fluent.Architecture.Interfaces
{
    public interface IFluentSpecificationOut : ISpec
    {
        Type FluentEntityType { get; }
        Type FluentEntityOutType { get; }
    }

    public interface IFluentSpecification<TO> : IFluentSpecificationOut
    {
    }
}