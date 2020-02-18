// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------


using dn32.infra.Services;
using System;

namespace dn32.infra.Interfaces
{
    public interface IFluenteSpecification : ISpec
    {
        Type FluenteEntityType { get; }
    }

    public interface ISpec
    {
        TransactionalService Service { get; set; }
    }
}
