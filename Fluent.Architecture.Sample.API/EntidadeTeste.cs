using Fluent.Architecture.EntityFramework.MySQL;
using Fluent.Architecture.EntityFramework.SqlServer;

namespace Fluent.Architecture.Sample
{
    public class EntidadeMySQL : FluentMySQLEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class EntidadeSqlServer : FluentSQLEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
