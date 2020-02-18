// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System.Reflection;

namespace dn32.infra.Nucleo.Models
{

    public class KeyValue
    {
        public PropertyInfo Property { get; set; }
        public object Value { get; set; }
        public string ColumnName { get; set; }
    }
}
