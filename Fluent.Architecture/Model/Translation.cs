// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fluent.Architecture.Model
{
    public class Translation : FluentEntity
    {
        [Key, Column(Order = 0)]
        public string Language { get; set; }

        [Key, Column(Order = 1)]
        public string EntityType { get; set; }

        [Key, Column(Order = 2)]
        public int EntityId { get; set; }

        [Key, Column(Order = 3)]
        public string Property { get; set; }

        public string Value { get; set; }
    }
}
