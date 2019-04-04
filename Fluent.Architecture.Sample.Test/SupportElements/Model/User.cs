// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;
using Fluent.Architecture.Test.Mock;
using Newtonsoft.Json;

namespace Fluent.Architecture.Sample.Test.SupportElements.Model
{
    [Table("users")]
    public class User : FluentIdEntity
    {
        [Key, Column(Order = 1)]
        public ePersonType? PersonType { get; set; }

        [Required, JsonProperty("full_name")]
        public string Name { get; set; }

        [FluentUniqueKey]
        public string UserName { get; set; }

        [FluentUniqueKey]
        public string Email { get; set; }

        public string Tel { get; set; }

        public string Password { get; set; }
    }
}