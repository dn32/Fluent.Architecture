// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------


using dn32.infra.Services;
using System;

namespace dn32.infra.Interfaces
{
    public interface IDnSpecification : ISpec
    {
        Type DnEntityType { get; }
    }

    public interface ISpec
    {
        TransactionalService Service { get; set; }
    }
}
