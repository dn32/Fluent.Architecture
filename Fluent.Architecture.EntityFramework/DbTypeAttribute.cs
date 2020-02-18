using System;

namespace dn32.infra.EntityFramework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class DbTypeAttribute : Attribute
    {
        public DnDbType DbType { get; set; }

        public string Identifier { get; set; }

        public DbTypeAttribute(DnDbType dbType)
        {
            DbType = dbType;
        }

        public DbTypeAttribute(DnDbType dbType, string identifier)
        {
            DbType = dbType;
            Identifier = identifier;
        }
    }
}
