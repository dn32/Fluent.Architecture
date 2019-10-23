// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class EntityExistsFluentValidationException : FluentValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "EntityExistsFluentValidationException";

        public EntityExistsFluentValidationException(string entityKeys) : base($"An entity with any of these keys already exists in the database: {entityKeys}", false, entityKeys)
        {
        }
    }
}