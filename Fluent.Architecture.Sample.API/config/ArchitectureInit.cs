using Fluent.Architecture;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.SqLite;
using System;

public class ArchitectureInit
{
    public static void Setup(IServiceProvider serviceProvider)
    {
        Fluent.Architecture.Setup
        .Init()
        .SetServiceProvider(serviceProvider)
        .UseEntityFramework()
        .AddConnectionString("Data Source=sample.db;", createDatabaseIfNotExists: true, typeof(EfContextSqLite))
        .Build()
        .Run();
    }
}