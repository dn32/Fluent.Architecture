
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace Fluent.Architecture.Core.Models
{
    public class FluentEventEntity
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public List<FluentEventEntityProperty> Properties { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public object CurrentEntity { get; set; }
        public Type CurrentEntityType { get; set; }
        internal EntityEntry ChangedEntity { get; set; }
    }
}
