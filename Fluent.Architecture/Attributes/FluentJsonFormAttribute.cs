using Newtonsoft.Json;
using System;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FluenteJsonFormAttribute : FluenteJsoSchemaAttribute
    {
        [JsonIgnore]
        public Type Type { get; set; }

        public bool IsIntermediateTable { get; set; }

        public bool IsReadOnly { get; set; }
    }
}
