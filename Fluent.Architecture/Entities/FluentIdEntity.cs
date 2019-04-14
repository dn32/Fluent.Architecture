// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fluent.Architecture.Entities
{
    public abstract class FluentIdEntity : FluentEntity
    {
        [Key, Column(Order = 0)]
        public int Id { get; set; }
    }
}