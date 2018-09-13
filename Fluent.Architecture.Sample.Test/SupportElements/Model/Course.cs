using Fluent.Architecture.Model;

namespace Fluent.Architecture.Sample.Test.SupportElements.Model
{
    /// <inheritdoc />
    public class Course : FluentGlobalizedEntity
    {
        [FluentGlobalization]
        public string Title { get; set; }

        [FluentGlobalization]
        public string Description { get; set; }
    }
}
