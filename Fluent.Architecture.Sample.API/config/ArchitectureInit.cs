using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.MySQL;
using Fluent.Architecture.EntityFramework.Oracle;
using Fluent.Architecture.EntityFramework.PostgreSQL;
using Fluent.Architecture.EntityFramework.SqlServer;
using System;

namespace Fluent.Architecture.Sample
{
    public class ArchitectureInit
    {
        public static void Setup(IServiceProvider serviceProvider)
        {
            Architecture.Setup
                .Init()
                .AddConnectionString(GetPostgreConnectionString, false, typeof(EfContextPostgreSQL))
                .AddConnectionString(GetSQLServerConnectionString, false, typeof(EfContextSQLServer))
                .AddConnectionString(GetMySQLConnectionString, false, typeof(EfContextMySQL))
                .AddConnectionString(GetOracleConnectionString, false, typeof(EfContextOracle))
                .SetServiceProvider(serviceProvider)
                .UseEntityFramework()
                .Build()
                .Run();
        }

        public static string GetSQLServerConnectionString(UserSessionRequest userSessionRequest)
        {
            return "Server=N000967\\MSSQLSERVER01;Database=dbTeste;Trusted_Connection=True;";
        }

        public static string GetPostgreConnectionString(UserSessionRequest userSessionRequest)
        {
            return "";
        }

        public static string GetMySQLConnectionString(UserSessionRequest userSessionRequest)
        {
            return "Server=localhost;Database=dbTeste;Uid=root;Pwd=admin;";
        }

        public static string GetOracleConnectionString(UserSessionRequest userSessionRequest)
        {
            return "User ID=TESTEMANUAL; Password=mxma#maxpedidonuvem; Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.62.38.63)(PORT=1721))(CONNECT_DATA=(SID = XE)))";
        }
    }
}
