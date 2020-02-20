using System;

namespace dn32.infra.nucleo.excecoes
{
    public class DesenvolvimentoIncorretoException : Exception
    {
        public DesenvolvimentoIncorretoException(string mensagem) : base(mensagem)
        {
        }
    }
}
