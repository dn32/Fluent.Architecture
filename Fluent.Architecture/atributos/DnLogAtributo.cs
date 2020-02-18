using System;
using dn32.infra.enumeradores;

namespace dn32.infra.nucleo.atributos
{
    public class DnLogAtributo : Attribute
    {
        public EnumApresentar Apresentar { get; set; }

        public DnLogAtributo(EnumApresentar apresentar = EnumApresentar.Mostrar)
        {
            this.Apresentar = apresentar;
        }
    }
}