// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;
using System;

namespace Fluent.Architecture.Factory
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
        /// <typeparam name="T">
        /// Tipo da entidade referente à validação desejada.
        /// </typeparam>
        /// <returns>
        /// A validação criada.
        /// </returns>
        internal static FluentValidation<T> Create<T>() where T : BaseEntity
        {
            var localType = Setup.Config?.Config?.GenericValidationType?.MakeGenericType(typeof(T)) ?? typeof(FluentValidation<T>);
            return Create(localType) as FluentValidation<T>;
        }

        internal static TransactionalValidation Create(Type validationType)
        {
            var localType = validationType;
            var entityType = validationType.GetFluentEntityType();

            if (entityType != null)
            {
                if (Setup.Validations.TryGetValue(entityType, out var validationTypeOut))
                {
                    localType = validationTypeOut;
                }
            }

            return Activator.CreateInstance(localType) as TransactionalValidation;
        }

    }
}
