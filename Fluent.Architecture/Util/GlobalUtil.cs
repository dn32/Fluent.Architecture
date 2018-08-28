// ReSharper disable CommentTypo

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Attributes;
using Fluent.Architecture.Controllers;
using Fluent.Architecture.Exception;
using Fluent.Architecture.Model;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Services;
using Fluent.Architecture.Specifications;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Util
{
    /// <summary>
    /// Utilitários de uso global.
    /// </summary>
    public static class GlobalUtil
    {
        /// <summary>
        /// Obtem o tipo da entidade de um objeto baseado em um tipo esperado de Fluent. Ex <see cref="FluentService{T}"/>, <see cref="FluentRepository{T}"/>, etc. O retorno será o tipo de T.
        /// </summary>
        /// <param name="objectTypeToCheck">
        /// Objeto a ser avaliado.
        /// </param>
        /// <param name="expectedType">
        /// Tipo esperado. Exemplo:  <see cref="FluentService{T}"/>, <see cref="FluentRepository{T}"/>
        /// </param>
        /// <returns>
        /// O tipo.
        /// </returns>
        internal static Tuple<string, Type> GetFluentEntityType(Type objectTypeToCheck, Type expectedType)
        {
            return new Tuple<string, Type>(GetBase(objectTypeToCheck.BaseType), objectTypeToCheck);

            string GetBase(Type type)
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
                    return args.Length == 0 ? type.Name : type.GetGenericArguments()[0].Name;
                }

                return GetBase(type.BaseType);
            }
        }

        /// <summary>
        /// Obtem o tipo da entidade de um tipo Fluent. Ex <see cref="FluentService{T}"/>. O tipo a ser encontrado é o tipo de T.
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
                if (type == null)
                {
                    return null;
                }

                if (type == typeof(object))
                {
                    return null;
                }

                if (
                    type.Name == typeof(FluentController<FluentEntity>).Name ||
                    type.Name == typeof(FluentService<FluentEntity>).Name ||
                    type.Name == typeof(FluentRepository<FluentEntity>).Name ||
                    type.Name == typeof(FluentValidation<FluentEntity>).Name ||
                    type.Name == typeof(FluentController<FluentEntity>).Name ||
                    type.Name == typeof(FluentSpecification<FluentEntity>).Name)
                {
                    return type.GetGenericArguments()[0];
                }

                return GetBase(type.BaseType);
            }
        }

        //Todo doc
        public static MethodBase GetMethodForPropagation()
        {
            var frames = new StackTrace().GetFrames();

            foreach (var frame in frames)
            {
                var method = frame.GetMethod();
                var type = method.ReflectedType;
                if (type != null && !type.IsAbstract && type.IsSubclassOf(typeof(BaseController)))
                {
                    return method;
                }
            }

            throw new IncorrectDevelopmentException("The propagation call could not be traced. Only BaseController child controllers can make propagation call.");
        }
    }
}
