// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fluent.Architecture.Extensions;

namespace Fluent.Architecture.Core.Models
{
    /// <summary>
    /// A entidade base de todas as entidades do sistema.
    /// </summary>
    public abstract class BaseEntity
    {
        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            var type = GetType();
            var keyElements = type.GetProperties().Where(x => x.GetCustomAttributeAny<KeyAttribute>()).ToList();
            var json = type.GetHashCode() + Newtonsoft.Json.JsonConvert.SerializeObject(keyElements.Select(x => x.GetValue(this)).ToArray());
            return json.GetHashCode();
        }
    }
}
