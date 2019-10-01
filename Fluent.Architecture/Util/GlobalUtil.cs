// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Controllers;
using Fluent.Architecture.Core.Interfaces;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Validation;
using System;
using System.Linq;

namespace Fluent.Architecture.Util
{
    /// <summary>
    /// Utilitários de uso global.
    /// </summary>
    public static class GlobalUtil
    {
        /// <summary>
        /// Obtem o tipo da entidade de um objeto baseado em um tipo esperado. Ex <see cref="FluentService{T}"/>, <see cref="FluentRepository{TE}"/>, etc. O retorno será o tipo de T.
        /// </summary>
        /// <param name="objectTypeToCheck">
        /// Objeto a ser avaliado.
        /// </param>
        /// <param name="expectedType">
        /// Tipo esperado. Exemplo:  <see cref="FluentService{T}"/>, <see cref="FluentRepository{TE}"/>
        /// </param>
        /// <returns>
        /// O tipo.
        /// </returns>
        internal static Tuple<Type, Type> GetFluentEntityType(Type objectTypeToCheck, Type expectedType)
        {
            return new Tuple<Type, Type>(GetBase(objectTypeToCheck.BaseType ?? objectTypeToCheck), objectTypeToCheck);

            Type GetBase(Type type)
            {
                if (type == typeof(object))
                {
                    throw new InvalidOperationException($"{objectTypeToCheck.Name} is not FLuentEntity");
                }

                if (type.Name == expectedType.Name)
                {
                    var args = type.GetGenericArguments();
                    return args.Length == 0 ? type : type.GetGenericArguments()[0];
                }

                return GetBase(type.BaseType ?? type);
            }
        }

        internal static Tuple<Type, Type> GetFluentEntityTypeByInterface(Type objectTypeToCheck, Type expectedType)
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

        private static string[] FluentEntityNames => new[]
        {
            typeof(FluentController<FluentEntity>).Name,
            typeof(FluentService<FluentEntity>).Name,
            typeof(IFluentRepository<FluentEntity>).Name,
            typeof(FluentValidation<FluentEntity>).Name,
            typeof(FluentSpecification<FluentEntity>).Name
        };

        /// <summary>
        /// Obtem o tipo da entidade de um tipo. Ex <see cref="FluentService{T}"/>. O tipo a ser encontrado é o tipo de T.
        /// </summary>
        /// <param name="currentType">
        /// Objeto a ser avaliado.
        /// </param>
        /// <returns>
        /// O tipo.
        /// </returns>
        public static Type GetFluentEntityType(this Type currentType)
        {
            return GetBase(currentType);

            Type GetBase(Type type)
            {
                if (type == null || type == typeof(object))
                {
                    throw new InvalidOperationException($"{currentType.Name} is not FluentEntity");
                }

                if (!FluentEntityNames.Contains(type.Name))
                {
                    return GetBase(type.BaseType ?? type);
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
