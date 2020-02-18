using dn32.infra.Nucleo.Atributos;
using System.Collections.Generic;

namespace dn32.infra.Nucleo.Models
{
    public class FluenteJsonSchema
    {
        public FluenteJsonFormAttribute FluenteJsonForm { get; set; }
        public List<FluenteJsonPropertyAttribute> Properties { get; set; }
    }
}
