
namespace Fluent.Architecture.Model
{
    public class FluentEventEntityProperty
    {
        public string PropertyBane { get; set; }
        public object OriginalValue { get; set; }
        public object CurrentValue { get; set; }
        public bool Changed => OriginalValue?.ToString() != CurrentValue?.ToString();
    }
}
