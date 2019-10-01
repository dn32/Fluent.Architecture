
namespace Fluent.Architecture.Entities
{
    public class FluentEventEntityProperty
    {
        public string PropertyName { get; set; }
        public object OriginalValue { get; set; }
        public object CurrentValue { get; set; }
        public bool Changed => OriginalValue?.ToString() != CurrentValue?.ToString();
    }
}
