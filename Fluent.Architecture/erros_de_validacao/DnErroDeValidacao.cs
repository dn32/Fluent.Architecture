using dn32.infra.Util;
using Newtonsoft.Json;
using System.Threading;

namespace dn32.infra.nucleo.erros_de_validacao
{
    public class DnErroDeValidacao
    {
        private readonly string chaveDeGlobalizacao;

        [JsonProperty("erro_de_validacao")]
        public bool ErroDeValidacao => true;

        [JsonProperty("mensagem")]
        public virtual string Mensagem { get; set; }

        [JsonProperty("chave_de_globalizacao")]
        public virtual string ChaveDeGlobalizacao => string.IsNullOrEmpty(this.chaveDeGlobalizacao) ? Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.Mensagem.ToLower()).ClearText() : this.chaveDeGlobalizacao;

        [JsonProperty("mensagem_de_globalizacao")]
        public virtual string MensagemDeGlobalizacao { get; set; }

        [JsonProperty("valores")]
        public virtual string[] Valores { get; set; }

        [JsonProperty("valores_globalizados")]
        public bool ValoresGlobalizados { get; }

        [JsonProperty("nome_do_erro_de_validacao")]
        public string NomeDoErroDeValidacao => GetType().Name;

        public DnErroDeValidacao(
            string mensagem,
            string chaveDeGlobalizacaoParam, 
            bool valoresGlobalizados = false, 
            params string[] valores)
        {
            this.chaveDeGlobalizacao = chaveDeGlobalizacaoParam;
            this.Mensagem = mensagem;
            this.Valores = valores;
            this.ValoresGlobalizados = valoresGlobalizados;
        }

        public DnErroDeValidacao(
            string mensagem,
            bool valoresGlobalizados = false,
            params string[] valores)
        {
            this.Mensagem = mensagem;
            this.Valores = valores;
            this.ValoresGlobalizados = valoresGlobalizados;
        }
    }
}