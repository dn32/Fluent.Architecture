// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace dn32.infra.Interfaces
{
    public interface IDnInclusionEntity
    {
        [NotMapped, JsonIgnore]
        string[] InclusionsForList { get; }

        [NotMapped, JsonIgnore]
        string[] InclusionsForOne { get; }
    }
}
