// ReSharper disable CommentTypo

using System.Reflection;

namespace Fluent.Architecture.Model
{

    public class KeyValue
    {
        public PropertyInfo Property { get; set; }
        public object Value { get; set; }
        public string ColumnName { get; set; }
    }
}
