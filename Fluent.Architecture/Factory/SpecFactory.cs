using dn32.infra.nucleo.excecoes;
using dn32.infra.servicos;
using dn32.infra.Specifications;
using System;

namespace dn32.infra.Factory
{
    public static class SpecFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam Nome="T">Tipo de serviço.</typeparam>
        /// <param Nome="service"></param>
        /// <returns></returns>
        public static T Criar<T>(DnServicoTransacionalBase service) where T : BaseSpecification
        {
            if (!(Activator.CreateInstance(typeof(T)) is T ts))
            {
                throw new DesenvolvimentoIncorretoException($"Failed to initialize specification [{typeof(T).Name}] type with specified constructor parameters not found.");
            }

            ts.SetService(service);
            return ts;
        }
    }
}
