// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------


using Fluente.Arquitetura.Services;
using System;

namespace Fluente.Arquitetura.Interfaces
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
