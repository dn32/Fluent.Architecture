
using System.Collections.Generic;

namespace Fluent.Architecture.Entities
{
    public class FluentEventEntity
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public List<FluentEventEntityProperty> Properties { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public object CurrentEntity { get; set; }
    }
}
