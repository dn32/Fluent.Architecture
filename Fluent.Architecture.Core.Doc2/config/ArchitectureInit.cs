using Fluent.Architecture;
using System;

public class ArchitectureInit
{
    public static void Setup(IServiceProvider serviceProvider)
    {
        Fluent.Architecture.Setup
            .Init()
            .SetServiceProvider(serviceProvider)
            .Build()
            .Run();
    }
}