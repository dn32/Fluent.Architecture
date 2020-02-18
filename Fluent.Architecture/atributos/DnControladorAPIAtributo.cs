using System;

namespace dn32.infra.nucleo.atributos
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DnControladorApiAtributo : Attribute
    {
        public bool GerarAutomaticamente { get; set; }

        public DnControladorApiAtributo()
        {
            GerarAutomaticamente = true;
        }

        public DnControladorApiAtributo(bool gerarAutomaticamente = true)
        {
            GerarAutomaticamente = gerarAutomaticamente;
        }
    }
}
