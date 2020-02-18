// -----------------------------------------------------------------------
// <copyright company="DnControlador System">
//     Copyright © DnControlador System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Newtonsoft.Json;

namespace dn32.infra.Exceptions.ValidationException
{
    public class EntityExistsDnValidationException : DnValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "EntityExistsDnValidationException";

        public EntityExistsDnValidationException(string entityKeys) : base($"An entity with any of these keys already exists in the database: {entityKeys}", false, entityKeys)
        {
        }
    }
}