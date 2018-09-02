// ReSharper disable CommentTypo
using System;
using Fluent.Architecture.Model;
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
        public static FluentValidation<T> Create<T>() where T : BaseEntity
        {
            Type localType;

            if (typeof(T).IsSubclassOf(typeof(FluentGlobalizedEntity)))
            {
                localType = typeof(FluentGlobalizedValidation<>).MakeGenericType(typeof(T));
            }
            else
            {
                localType = typeof(FluentValidation<T>);
            }

            if (Setup.Validations.TryGetValue(typeof(T).Name, out var validationType))
            {
                localType = validationType;
            }

            return Activator.CreateInstance(localType) as FluentValidation<T>;
        }
    }
}
