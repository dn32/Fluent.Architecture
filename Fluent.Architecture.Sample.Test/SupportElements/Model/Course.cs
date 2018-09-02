using Fluent.Architecture.Model;

namespace Fluent.Architecture.Sample.Test.SupportElements.Model
{
    /// <inheritdoc />
    public class Course : FluentGlobalizedEntity
    {
        public int Id { get; set; }

        [FluentGlobalization]
        public string Title { get; set; }

        [FluentGlobalization]
        public string Description { get; set; }
    }
}
