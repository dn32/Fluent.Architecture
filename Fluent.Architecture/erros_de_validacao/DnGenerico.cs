using System.Reflection;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class DnGenericoErroDeValidacao : DnCampoDeTelaErroDeValidacao
    {
        public DnGenericoErroDeValidacao(
            PropertyInfo propriedade,
            bool valoresGlobalizados,
            string mensagem,
            string propriedadeDeComposicao,
            string campoDeComposicao) :
            base(
                propriedade,
                valoresGlobalizados,
                mensagem,
                propriedadeDeComposicao,
                campoDeComposicao)
        {
        }
    }
}