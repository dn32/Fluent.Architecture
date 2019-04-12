using Fluent.Architecture.EntityFramework.MySQL;
using Fluent.Architecture.EntityFramework.PostgreSQL;
using Fluent.Architecture.EntityFramework.SqlServer;
using Fluent.Architecture.Enumerator;
using System;

namespace Fluent.Architecture.Sample
{
    public class ArchitectureInit
    {
        public static void Setup(IServiceProvider serviceProvider)
        {
            Architecture.Setup
                .Init()
                //.AddConnectionString(GetSQLServerConnectionString, true, FluentDbType.SQL_SERVER, typeof(EfContextSQL))
                //.AddConnectionString(GetPostgreConnectionString, true, FluentDbType.POSTGREE_SQL, typeof(EfContextPostgreSQL))
                .AddConnectionString(GetMySQLConnectionString, true, FluentDbType.MYSQL, typeof(EfContextMySQL))
                .SetServiceProvider(serviceProvider)
                .Build()
                .Run();
        }

        public static string GetSQLServerConnectionString(object sessionId)
        {
            return "Server=N000967\\MSSQLSERVER01;Database=dbTeste;Trusted_Connection=True;";
        }

        public static string GetPostgreConnectionString(object sessionId)
        {
            return "";
        }

        public static string GetMySQLConnectionString(object sessionId)
        {
            return "Server=localhost;Database=dbTeste;Uid=root;Pwd=admin;";
        }
    }
}
