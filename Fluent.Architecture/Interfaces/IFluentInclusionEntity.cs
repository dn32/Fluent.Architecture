// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Fluent.Architecture.Interfaces
{
    public class Attr2 : Attribute { }
    public interface IFluentInclusionEntity
    {
        [NotMapped, JsonIgnore, Attr2]
        string[] InclusionsForList { get; }

        [NotMapped, JsonIgnore, Attr2]
        string[] InclusionsForOne { get; }
    }
}
