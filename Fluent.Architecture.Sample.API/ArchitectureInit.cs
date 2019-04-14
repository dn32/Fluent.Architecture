using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.MySQL;
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
                //.AddConnectionString(GetSQLServerConnectionString, true, typeof(EfContextSQL))
                //.AddConnectionString(GetPostgreConnectionString, true, typeof(EfContextPostgreSQL))
                .AddConnectionString(GetMySQLConnectionString, true, typeof(EfContextMySQL))
                .SetServiceProvider(serviceProvider)
                .UseEntityFramework()
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
