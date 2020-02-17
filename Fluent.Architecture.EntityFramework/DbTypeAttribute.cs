using System;

namespace Fluente.Arquitetura.EntityFramework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DbTypeAttribute : Attribute
    {
        public FluenteDbType DbType { get; set; }

        public string Identifier { get; set; }

        public DbTypeAttribute(FluenteDbType dbType)
        {
            DbType = dbType;
        }

        public DbTypeAttribute(FluenteDbType dbType, string identifier)
        {
            DbType = dbType;
            Identifier = identifier;
        }
    }
}
