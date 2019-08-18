// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Attributes;
using Fluent.Architecture.Entities;
using System.Collections.Generic;

namespace Fluent.Architecture.Sample.Test.SupportElements.Model
{
    [NotDbEntity]
    public class TestEntity : FluentEntity
    {
        public int Id { get; set; }

        public List<int> Data { get; set; }

        public List<int> Data2 { get; set; }

#pragma warning disable 414
        private readonly string _internalString = "my value";
#pragma warning restore 414

        public TestEntity2 TestEntity2 { get; set; } = new TestEntity2();
    }
}
