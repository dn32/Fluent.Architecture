// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace dn32.infra.Interfaces
{
    public interface IDnSpecificationOut : ISpec
    {
        Type DnEntityType { get; }
        Type DnEntityOutType { get; }
    }

    public interface IDnSpecification<TO> : IDnSpecificationOut
    {
    }
}