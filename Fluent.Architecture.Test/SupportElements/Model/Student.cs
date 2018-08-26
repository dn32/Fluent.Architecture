using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Test.SupportElements.Model
{
    public class Student : FluentEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [FluentUnicKey]
        public string Document { get; set; }

        public string Email { get; set; }
    }
}
