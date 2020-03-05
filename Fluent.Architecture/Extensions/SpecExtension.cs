using dn32.infra.nucleo.excecoes;
using dn32.infra.Interfaces;
using System.Collections.Generic;
using dn32.infra.dados;

namespace dn32.infra.extensoes
{
    public static class SpecExtension
    {
        public static bool Exists(this ISpec spec)
        {
            return (bool)spec.Execute(nameof(Exists));
        }

        public static object List(this ISpec spec, DnPaginacao pagination = null)
        {
            return spec.Execute(nameof(List), new object[] { pagination });
        }

        public static object FirstOrDefault(this ISpec spec)
        {
            return spec.Execute(nameof(FirstOrDefault));
        }

        public static object Execute(this ISpec spec, string method, params object[] parameters)
        {
            return spec.CallSelectOrNoSelectMethod(method, parameters);
        }

        public static object CallSelectOrNoSelectMethod(this ISpec spec, string methodName, params object[] parameters)
        {
            var paramList = new List<object> { spec };
            paramList.AddRange(parameters);
            parameters = paramList.ToArray();

            if (spec != null && spec.Service == null)
            {
                throw new DesenvolvimentoIncorretoException($"The past spec does not have a valid service. See at the time the spec is created if a service has been passed in the spec creator.");
            }

            if (spec is IDnSpecificationOut spec2)
            {
                var service = spec2.DnEntityType.GetServiceInstanceByEntity(spec2.Service.SessaoDaRequisicao);
                var method = service.GetType().GetMethod($"{methodName}Select");
                if (method == null)
                {
                    throw new DesenvolvimentoIncorretoException($"Method not found: {methodName}Select");
                }

                return method.MakeGenericMethod(spec2.DnEntityOutType).Invoke(service, parameters);
            }

            if (spec is IDnSpecification spec3)
            {
                var service = spec3.DnEntityType.GetServiceInstanceByEntity(spec3.Service.SessaoDaRequisicao);
                var method = service.GetType().GetMethodWithoutAmbiguity(methodName, parameters);
                if (method == null)
                {
                    throw new DesenvolvimentoIncorretoException($"Method not found: {methodName}");
                }

                return method.Invoke(service, parameters);
            }

            throw new DesenvolvimentoIncorretoException("The specification is of a different type than expected");
        }

        //public static T Adicionar<T>(this TransactionalService service, T entity) where T : BaseEntity
        //{
        //    return service.Adicionar(entity);
        //}

        //public static bool Exists(this TransactionalService service, IDnSpecification spec)
        //{
        //    return spec.Exists();
        //}

        //public static bool Exists<TO>(this TransactionalService service, IDnSpecification<TO> spec)
        //{
        //    return spec.Exists();
        //}

        //public static void AdicionarLista<T>(this TransactionalService service, params T[] entities) where T : BaseEntity
        //{
        //    return service.AdicionarLista(entities);
        //}

        //public static int Quantidade<TO>(this TransactionalService service, IDnSpecification<TO> spec)
        //{
        //    return service.Quantidade(spec);
        //}

        ////public static int Quantidade(this TransactionalService service)
        ////{
        ////    return service.Quantidade(spec);
        ////}

        //public static int Quantidade(this TransactionalService service, IDnSpecification spec)
        //{
        //    return spec.Quantidade();
        //}


        //public static T Find<T>(this TransactionalService service, T entity) where T : BaseEntity
        //{
        //    return service.Find(entity);
        //}

        //public static TO FirstOrDefault<TO>(this TransactionalService service, IDnSpecification<TO> spec)
        //{
        //    return service.FirstOrDefault(entity);
        //}

        //public static T FirstOrDefault<T>(this TransactionalService service, IDnSpecification spec) where T : BaseEntity
        //{
        //    return service.FirstOrDefault(entity);
        //}

        //public static T FirstOrDefault<T>(this TransactionalService service) where T : BaseEntity
        //{
        //    return service.FirstOrDefault(entity);
        //}

        //public static Listar<TO> Listar<TO>(this TransactionalService service, IDnSpecification<TO> spec, DnPagination pagination = null)
        //{
        //    return service.Listar(spec, pagination);
        //}

        //public static Listar<T> Listar<T>(this TransactionalService service, IDnSpecification spec, DnPagination pagination = null) where T : BaseEntity
        //{
        //    return service.Listar(spec, pagination);
        //}

        //public static T Remover<T>(this TransactionalService service, T entity) where T : BaseEntity
        //{
        //    return service.Remover(entity);
        //}

        //public static void RemoverLista(this TransactionalService service, IDnSpecification spec)
        //{
        //    service.RemoverLista(spec);
        //}

        //public static void RemoverLista<T>(this TransactionalService service, params T[] entities) where T : BaseEntity
        //{
        //    service.AdicionarLista(entities);
        //}

        //public static T Atualizar<T>(this TransactionalService service, T entity) where T : BaseEntity
        //{
        //    return service.Atualizar(entity);
        //}

        #region PRIVATES

        //internal static DnSpecification<TE> GetSpec<TE>(this IDnSpecification spec) where TE : BaseEntity
        //{
        //    return spec as DnSpecification<TE>;
        //}

        //private static DnControladorDeServico<TE> GetService<TE>(UserSessionRequest sessionRequest) where TE : BaseEntity
        //{
        //    return typeof(TE).GetServiceInstanceByEntity(sessionRequest) as DnControladorDeServico<TE>;
        //}

        #endregion
    }
}
