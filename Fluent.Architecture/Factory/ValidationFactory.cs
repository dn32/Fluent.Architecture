using dn32.infra.Util;
using dn32.infra.Validation;
using System;
using dn32.infra.dados;

namespace dn32.infra.Factory
{
    /// <summary>
    /// Método interno.
    /// Fábrica de validações.
    /// </summary>
    internal class ValidationFactory
    {
        /// <summary>
        /// Cria uma nova validação.
        /// </summary>
        /// <typeparam Nome="T">
        /// Tipo da entidade referente à validação desejada.
        /// </typeparam>
        /// <returns>
        /// A validação criada.
        /// </returns>
        internal static DnValidation<T> Create<T>() where T : EntidadeBase
        {
            var localType = Setup.ConfiguracoesGlobais.GenericValidationType?.MakeGenericType(typeof(T)) ?? typeof(DnValidation<T>);
            return Create(localType) as DnValidation<T>;
        }

        internal static TransactionalValidation Create(Type validationType)
        {
            var localType = validationType;
            var entityType = validationType.GetDnEntityType();

            if (entityType != null)
            {
                if (Setup.Validacoes.TryGetValue(entityType, out var validationTypeOut))
                {
                    localType = validationTypeOut;
                }
            }

            return Activator.CreateInstance(localType) as TransactionalValidation;
        }

    }
}
