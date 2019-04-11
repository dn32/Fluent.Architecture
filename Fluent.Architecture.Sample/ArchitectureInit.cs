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
                .AddConnectionString("conexao.com.br", FluentDbType.SQL_SERVER)
                .AddConnectionString(GetConnectionStringOracle, FluentDbType.ORACLE)
                .AddConnectionString(GetConnectionStringOracle2, FluentDbType.ORACLE, "ORACLE2")
                .SetServiceProvider(serviceProvider)
                .Build()
                .Run();
        }

        public static string GetConnectionStringOracle(object sessionId)
        {
            return "conn";
        }

        public static string GetConnectionStringOracle2(object sessionId)
        {
            return "conn";
        }
    }
}
