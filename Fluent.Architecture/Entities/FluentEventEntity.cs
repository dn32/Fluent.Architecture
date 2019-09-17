
using System.Collections.Generic;

namespace Fluent.Architecture.Entities
{
    public class FluentEventEntity
    {
        public List<FluentEventEntityProperty> Properties { get; set; }
        public object CurrentEntity { get; set; }
    }
}
