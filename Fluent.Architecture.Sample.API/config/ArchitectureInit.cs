using Fluent.Architecture;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.MySQL;
using Fluent.Architecture.EntityFramework.SqLite;
using Fluent.Architecture.EntityFramework.SqlServer;
using System;

public class ArchitectureInit
{
    public static void Setup(IServiceProvider serviceProvider)
    {
        Fluent.Architecture.Setup
            .Init()
            .UseEntityFramework()
            .AddConnectionString("Data Source=sample.db;", createDatabaseIfNotExists: true, typeof(EfContextSqLite))
            .AddConnectionString("Server=51.83.33.154;Database=sample;Uid=myUsername;Pwd=myGcp1926*;", createDatabaseIfNotExists: true, typeof(EfContextMySQL))
            .AddConnectionString("Data Source=51.83.33.154;Initial Catalog=sample;User ID=sa;Password=miGcp1926*;", createDatabaseIfNotExists: true, typeof(EfContextSQLServer))
            .Build();
    }
}