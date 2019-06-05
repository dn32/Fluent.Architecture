using Fluent.Architecture;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.MySQL;
using System;

public class ArchitectureInit
{
    public static void Setup(IServiceProvider serviceProvider)
    {
        Fluent.Architecture.Setup
        .Init()
        .SetServiceProvider(serviceProvider)
        .UseEntityFramework()
        .AddConnectionString("Server=localhost;Database=testDb;Uid=root;Pwd=admin;", createDatabaseIfNotExists: true, typeof(EfContextMySQL))
        .Build()
        .Run();
    }
}