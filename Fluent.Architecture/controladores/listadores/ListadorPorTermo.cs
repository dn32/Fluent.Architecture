//using System.Collections.Generic;
//using System.Threading.Tasks;
//using dn32.infra.dados;
//using dn32.infra.Nucleo.Specifications;

//namespace dn32.infra.nucleo.controladores.listadores
//{
//    internal class ListadorPorTermo<T> : Listador<T> where T : DnEntidade, new()
//    {
//        public string Termo { get; set; }

//        public ListadorPorTermo(string termo)
//        {
//            this.Termo = termo;
//        }

//        public override Task<List<T>> Consultar()
//        {
//            var especificacao = this.CriarEspecificacao<TermSpec<T>>().SetParameter(this.Termo, isList: true);
//            return this.Servico.ListarAsync(especificacao);
//        }
//    }
//}