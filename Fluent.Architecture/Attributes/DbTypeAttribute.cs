using Fluent.Architecture.Enumerator;
using System;
using System.Data;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DbTypeAttribute : Attribute
    {
        public FluentDbType DbType { get; set; }

        public string  Identifier { get; set; }

        public DbTypeAttribute(FluentDbType dbType)
        {
            DbType = dbType;
        }

        public DbTypeAttribute(FluentDbType dbType, string identifier)
        {
            DbType = dbType;
            Identifier = identifier;
        }
    }
}
