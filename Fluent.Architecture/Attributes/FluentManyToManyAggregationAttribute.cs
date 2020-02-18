using System;

namespace dn32.infra.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteManyToManyAggregationAttribute : FluenteAggregationAttribute
    {
        public bool IsManyToMany { get; } = true;
    }
}
