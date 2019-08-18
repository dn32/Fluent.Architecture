// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------


using Fluent.Architecture.Services;
using System;

namespace Fluent.Architecture.Interfaces
{
    public interface IFluentSpecification : ISpec
    {
        Type FluentEntityType { get; }
    }

    public interface ISpec
    {
        TransactionalService Service { get; set; }
    }
}
