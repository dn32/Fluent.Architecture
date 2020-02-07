// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fluent.Architecture.Interfaces
{
    public interface IFluentInclusionEntity
    {
        [NotMapped, JsonIgnore]
        string[] InclusionsForList { get; }

        [NotMapped, JsonIgnore]
        string[] InclusionsForOne { get; }
    }
}
