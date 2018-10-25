using Fluent.Architecture.Enumerator;

namespace Fluent.Architecture.Model
{
    public abstract class BaseEvent
    {
        public object ObjectEvent { get; set; }

        public EnumEventType EventType { get; set; }

        public string EventName { get; set; }
    }
}
