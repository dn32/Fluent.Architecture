using Fluent.Architecture;
using System;

namespace Fluent.Architecture.Core.Doc.Sample
{
    public class ArchitectureInit
    {
        public static void Setup(IServiceProvider serviceProvider)
        {
            Fluent.Architecture.Setup.Init().Build();
        }
    }
}