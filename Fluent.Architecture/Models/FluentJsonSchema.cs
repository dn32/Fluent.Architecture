using Fluente.Arquitetura.Nucleo.Atributos;
using System.Collections.Generic;

namespace Fluente.Arquitetura.Nucleo.Models
{
    public class FluenteJsonSchema
    {
        public FluenteJsonFormAttribute FluenteJsonForm { get; set; }
        public List<FluenteJsonPropertyAttribute> Properties { get; set; }
    }
}
