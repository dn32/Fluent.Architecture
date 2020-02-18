// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace dn32.infra.Interfaces
{
    public interface IFluenteInclusionEntity
    {
        [NotMapped, JsonIgnore]
        string[] InclusionsForList { get; }

        [NotMapped, JsonIgnore]
        string[] InclusionsForOne { get; }
    }
}
