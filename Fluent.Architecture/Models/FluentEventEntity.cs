
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace dn32.infra.Nucleo.Models
{
    public class DnEventEntity
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public List<DnEventEntityProperty> Properties { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
        public object CurrentEntity { get; set; }
        public Type CurrentEntityType { get; set; }
        public EntityEntry ChangedEntity { get; set; }
        public EntityState EntityState { get; set; }
    }
}
