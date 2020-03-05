// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------


using dn32.infra.servicos;
using System;

namespace dn32.infra.Interfaces
{
    public interface IDnSpecification : ISpec
    {
        Type DnEntityType { get; }
    }

    public interface ISpec
    {
        DnServicoTransacionalBase Service { get; set; }
    }
}
