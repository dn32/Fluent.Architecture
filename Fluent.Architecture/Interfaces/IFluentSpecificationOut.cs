// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace dn32.infra.Interfaces
{
    public interface IFluenteSpecificationOut : ISpec
    {
        Type FluenteEntityType { get; }
        Type FluenteEntityOutType { get; }
    }

    public interface IFluenteSpecification<TO> : IFluenteSpecificationOut
    {
    }
}