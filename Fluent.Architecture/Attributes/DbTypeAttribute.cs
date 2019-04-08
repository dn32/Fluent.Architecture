using Fluent.Architecture.Enumerator;
using System;
using System.Data;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DbTypeAttribute : Attribute
    {
        public FluentDbType DbType { get; set; }
        public DbTypeAttribute(FluentDbType dbType)
        {
            DbType = dbType;
        }
    }
}
