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
                .AddConnectionString(GetConnectionString, true, FluentDbType.SQL_SERVER)
                .SetServiceProvider(serviceProvider)
                .Build()
                .Run();
        }

        public static string GetConnectionString(object sessionId)
        {
            return "Server=N000967\\MSSQLSERVER01;Database=dbTeste;Trusted_Connection=True;";
        }
    }
}
