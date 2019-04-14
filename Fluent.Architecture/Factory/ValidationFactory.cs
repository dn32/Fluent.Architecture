// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using System;
using Fluent.Architecture.Entities;
using Fluent.Architecture.Util;
using Fluent.Architecture.Validation;

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
            return Create(typeof(FluentValidation<T>)) as FluentValidation<T>;
        }

        internal static TransactionalValidation Create(Type validationType)
        {
            var localType = validationType;
            var entityType = validationType.GetFluentEntityType();

            if (entityType != null)
            {
                if (entityType.IsSubclassOf(typeof(FluentGlobalizedEntity)))
                {
                    localType = typeof(FluentGlobalizedValidation<>).MakeGenericType(entityType);
                }

                if (Setup.Validations.TryGetValue(entityType, out var validationTypeOut))
                {
                    localType = validationTypeOut;
                }
            }

            return Activator.CreateInstance(localType) as TransactionalValidation;
        }

    }
}
