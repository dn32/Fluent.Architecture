#if NET461
using System.Collections.Generic;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Test.InternalTests
{
    public partial class BaseServiceTest
    {
        [NotDbEntity]
        public class TestEntity : FluentEntity
        {
            public int Id { get; set; }

            public List<int> Data { get; set; }

            public List<int> Data2 { get; set; }

            private readonly string _internalString = "my value";

            public TestEntity2 TestEntity2 { get; set; } = new TestEntity2();
        }

        [NotDbEntity]
        public class TestEntity2 : FluentEntity
        {
            public int Id { get; set; }
        }
    }
}
#endif