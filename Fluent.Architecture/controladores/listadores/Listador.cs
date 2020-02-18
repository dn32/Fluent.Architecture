//using System.Collections.Generic;
//using System.Threading.Tasks;
//using dn32.infra.dados;
//using dn32.infra.Factory;
//using dn32.infra.Services;
//using dn32.infra.Specifications;

//namespace dn32.infra.nucleo.controladores.listadores
//{
//    internal abstract class Listador<T> where T : DnEntidade, new()
//    {
//        protected T2 CriarEspecificacao<T2>() where T2 : BaseSpecification
//        {
//            return SpecFactory.Create<T2>(this.Servico);
//        }

//        public DnService<T> Servico { get; set; }

//        public void AdicionarEspecificacao() { }

//        public void AdicionarServico() { }

//        public abstract Task<List<T>> Consultar();

//        public void ObterListagem() { }
//    }

//}
