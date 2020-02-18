using System;

namespace dn32.infra.nucleo.atributos
{
    public abstract class DnValidacaoAtributo : Attribute
    {
        public abstract bool EhValidoQuando(object valor);
        public abstract string MensagemDeInvalido { get; }
        public abstract object ValorDeExemplo { get; }
        public object Entidade { get; internal set; }
    }
}
