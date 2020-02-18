//using System.Collections.Generic;
//using System.Threading.Tasks;
//using dn32.infra.dados;
//using dn32.infra.Nucleo.Specifications;

//namespace dn32.infra.nucleo.controladores.listadores
//{
//    internal class ListadorTudo<T> : Listador<T> where T : DnEntidade, new()
//    {
//        public override async Task<List<T>> Consultar()
//        {
//            var especificacao = this.CriarEspecificacao<DnAllSpec<T>>().SetParameter(isList: true);
//            return await this.Servico.ListarAsync(especificacao);
//        }
//    }
//}