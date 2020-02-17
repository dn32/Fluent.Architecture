using System;

namespace Fluente.Arquitetura.Nucleo.Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluenteRandomKeyValueOnAddAttribute : Attribute
    {
        public int Max { get; set; }

        public FluenteRandomKeyValueOnAddAttribute(int max = 0)
        {
            Max = max;
        }
    }
}
