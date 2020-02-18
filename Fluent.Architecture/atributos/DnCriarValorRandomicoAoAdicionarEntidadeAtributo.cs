using System;

namespace dn32.infra.nucleo.atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DnCriarValorRandomicoAoAdicionarEntidadeAtributo : Attribute
    {
        public int TamanhoMaximo { get; set; }

        public DnCriarValorRandomicoAoAdicionarEntidadeAtributo(int tamanhoMaximo = 0)
        {
            this.TamanhoMaximo = tamanhoMaximo;
        }
    }
}
