using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Test.SupportElements.Model
{
    [NotDbEntity]
    public class TestEntity2 : FluentEntity
    {
        public int Id { get; set; }
    }
}