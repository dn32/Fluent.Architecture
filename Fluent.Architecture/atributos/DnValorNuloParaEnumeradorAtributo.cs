using System;

namespace dn32.infra.nucleo.atributos
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class DnValorNuloParaEnumeradorAtributo : Attribute
    {
        public int Valor { get; }

        public DnValorNuloParaEnumeradorAtributo(int valor)
        {
            this.Valor = valor;
        }
    }
}