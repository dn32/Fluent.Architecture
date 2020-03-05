using dn32.infra.nucleo.controladores;
using dn32.infra.Nucleo.Interfaces;
using dn32.infra.servicos;
using dn32.infra.Specifications;
using dn32.infra.Validation;
using System;
using System.Linq;
using dn32.infra.dados;

namespace dn32.infra.Util
{
    /// <summary>
    /// Utilitários de uso global.
    /// </summary>
    public static class GlobalUtil
    {
        /// <summary>
        /// Obtem o tipo da entidade de um objeto baseado em um tipo esperado. Ex <see cref="servicos.DnControladorDeServico{T}"/>, <see cref="DnRepository{TE}"/>, etc. O retorno será o tipo de T.
        /// </summary>
        /// <param Nome="objectTypeToCheck">
        /// Objeto a ser avaliado.
        /// </param>
        /// <param Nome="expectedType">
        /// Tipo esperado. Exemplo:  <see cref="servicos.DnControladorDeServico{T}"/>, <see cref="DnRepository{TE}"/>
        /// </param>
        /// <returns>
        /// O tipo.
        /// </returns>
        internal static Tuple<Type, Type> GetDnEntityType(Type objectTypeToCheck, Type expectedType)
        {
            return new Tuple<Type, Type>(GetBase(objectTypeToCheck.BaseType), objectTypeToCheck);

            Type GetBase(Type type)
            {
                if (type == null)
                {
                    return null;
                }

                if (type == typeof(object))
                {
                    return null;
                }

                if (type.Name == expectedType.Name)
                {
                    var args = type.GetGenericArguments();
                    return args.Length == 0 ? type : type.GetGenericArguments()[0];
                }

                return GetBase(type.BaseType);
            }
        }

        internal static Tuple<Type, Type> GetDnEntityTypeByInterface(Type objectTypeToCheck, Type expectedType)
        {
            var ints = objectTypeToCheck.GetInterfaces();
            var interface_ = ints.FirstOrDefault(x => x.Name == expectedType.Name);
            if (interface_ != null)
            {
                var args = interface_.GetGenericArguments();
                return args.Length == 0 ? null : new Tuple<Type, Type>(interface_.GetGenericArguments()[0], objectTypeToCheck);
            }

            return null;
        }

        private static string[] DnEntityNames => new[]
        {
            typeof(DnControlador<DnEntidade>).Name,
            typeof(servicos.DnServico<DnEntidade>).Name,
            typeof(IDnRepository<DnEntidade>).Name,
            typeof(DnValidation<DnEntidade>).Name,
            typeof(DnSpecification<DnEntidade>).Name
        };

        /// <summary>
        /// Obtem o tipo da entidade de um tipo. Ex <see cref="servicos.DnControladorDeServico{T}"/>. O tipo a ser encontrado é o tipo de T.
        /// </summary>
        /// <param Nome="currentType">
        /// Objeto a ser avaliado.
        /// </param>
        /// <returns>
        /// O tipo.
        /// </returns>
        public static Type GetDnEntityType(this Type currentType)
        {
            return GetBase(currentType);

            Type GetBase(Type type)
            {
                if (type == null || type == typeof(object))
                {
                    return null;
                }

                if (!DnEntityNames.Contains(type.Name))
                {
                    return GetBase(type.BaseType);
                }

                var localType = type.GetGenericArguments().First();
                if (!localType.IsSubclassOf(typeof(EntidadeBase)))
                {
                    throw new InvalidOperationException();
                }

                return localType;
            }
        }
    }
}
