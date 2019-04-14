// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using System.Reflection;

namespace Fluent.Architecture.Entities
{

    public class KeyValue
    {
        public PropertyInfo Property { get; set; }
        public object Value { get; set; }
        public string ColumnName { get; set; }
    }
}
