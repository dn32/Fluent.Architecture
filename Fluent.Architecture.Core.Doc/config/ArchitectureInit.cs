using Fluent.Architecture;
using System;

namespace Fluent.Architecture.Core.Doc.config
{
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
}