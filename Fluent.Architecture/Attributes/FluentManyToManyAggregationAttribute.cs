using System;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteManyToManyAggregationAttribute : FluenteAggregationAttribute
    {
        public bool IsManyToMany { get; } = true;
    }
}
