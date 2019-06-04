using Fluent.Architecture;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.MySQL;
using Fluent.Architecture.EntityFramework.Oracle;
using Fluent.Architecture.EntityFramework.PostgreSQL;
using System;

namespace Max.Infraestrutura.config
{
    public class Inicializacao
    {
        public static void Inicialize(IServiceProvider serviceProvider)
        {
            Setup
            .Init()
            .AddConnectionString(GetMySQLConnectionString, true, typeof(EfContextMySQL))
            .AddConnectionString(GetPostgreConnectionString, false, typeof(EfContextPostgreSQL))
            .AddConnectionString(GetOracleConnectionString, false, typeof(EfContextOracle))
            .SetServiceProvider(serviceProvider)
            .UseEntityFramework()
            .Build()
            .Run();
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
