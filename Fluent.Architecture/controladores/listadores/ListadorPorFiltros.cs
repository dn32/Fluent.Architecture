//using System.Collections.Generic;
//using System.Threading.Tasks;
//using dn32.infra.dados;
//using dn32.infra.Nucleo.Specifications;

//namespace dn32.infra.nucleo.controladores.listadores
//{
//    internal class ListadorPorFiltros<T> : Listador<T> where T : DnEntidade, new()
//    {
//        private Filtro[] Filtros { get; }

//        public ListadorPorFiltros(Filtro[] filtros)
//        {
//            this.Filtros = filtros;
//        }

//        public override async Task<List<T>> Consultar()
//        {
//            var especificacao = this.CriarEspecificacao<DnFilterSpec<T>>().SetParameter(this.Filtros, ehLista: true);
//            return await this.Servico.ListarAsync(especificacao);
//        }
//    }
//}