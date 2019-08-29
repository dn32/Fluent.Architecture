
namespace Fluent.Architecture.Core.Inconsistences
{
    public class FluentInconsistence
    {
        public string Message { get; set; }
        public string GlobalizationKey { get; set; }
    }

    public class FluentPropertyInconsistence : FluentInconsistence
    {
        public string PropertyName { get; set; }
    }

    public class FluentUiFieldInconsistence : FluentPropertyInconsistence
    {
        public string Field { get; set; }
    }
}
