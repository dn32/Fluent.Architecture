// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace Fluente.Arquitetura.Exceptions.ValidationException
{
    public class EntityExistsFluenteValidationException : FluenteValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "EntityExistsFluenteValidationException";

        public EntityExistsFluenteValidationException(string entityKeys) : base($"An entity with any of these keys already exists in the database: {entityKeys}", false, entityKeys)
        {
        }
    }
}