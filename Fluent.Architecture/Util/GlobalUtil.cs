// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluente.Arquitetura.Controllers;
using Fluente.Arquitetura.Nucleo.Interfaces;
using Fluente.Arquitetura.Nucleo.Models;
using Fluente.Arquitetura.Services;
using Fluente.Arquitetura.Specifications;
using Fluente.Arquitetura.Validation;
using System;
using System.Linq;

namespace Fluente.Arquitetura.Util
{
    /// <summary>
    /// Utilitários de uso global.
    /// </summary>
    public static class GlobalUtil
    {
        /// <summary>
        /// Obtem o tipo da entidade de um objeto baseado em um tipo esperado. Ex <see cref="FluenteService{T}"/>, <see cref="FluenteRepository{TE}"/>, etc. O retorno será o tipo de T.
        /// </summary>
        /// <param name="objectTypeToCheck">
        /// Objeto a ser avaliado.
        /// </param>
        /// <param name="expectedType">
        /// Tipo esperado. Exemplo:  <see cref="FluenteService{T}"/>, <see cref="FluenteRepository{TE}"/>
        /// </param>
        /// <returns>
        /// O tipo.
        /// </returns>
        internal static Tuple<Type, Type> GetFluenteEntityType(Type objectTypeToCheck, Type expectedType)
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

        internal static Tuple<Type, Type> GetFluenteEntityTypeByInterface(Type objectTypeToCheck, Type expectedType)
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

        private static string[] FluenteEntityNames => new[]
        {
            typeof(FluenteController<FluenteEntity>).Name,
            typeof(FluenteService<FluenteEntity>).Name,
            typeof(IFluenteRepository<FluenteEntity>).Name,
            typeof(FluenteValidation<FluenteEntity>).Name,
            typeof(FluenteSpecification<FluenteEntity>).Name
        };

        /// <summary>
        /// Obtem o tipo da entidade de um tipo. Ex <see cref="FluenteService{T}"/>. O tipo a ser encontrado é o tipo de T.
        /// </summary>
        /// <param name="currentType">
        /// Objeto a ser avaliado.
        /// </param>
        /// <returns>
        /// O tipo.
        /// </returns>
        public static Type GetFluenteEntityType(this Type currentType)
        {
            return GetBase(currentType);

            Type GetBase(Type type)
            {
                if (type == null || type == typeof(object))
                {
                    return null;
                }

                if (!FluenteEntityNames.Contains(type.Name))
                {
                    return GetBase(type.BaseType);
                }

                var localType = type.GetGenericArguments().First();
                if (!localType.IsSubclassOf(typeof(BaseEntity)))
                {
                    throw new InvalidOperationException();
                }

                return localType;
            }
        }
    }
}
