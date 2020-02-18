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
    public class EntityNotFoundDnValidationException : DnValidationException
    {
        public EntityNotFoundDnValidationException(string entityKeys)
            : base($"No entity with this key(s) was found in the database: {entityKeys}", false, entityKeys)
        {
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "NoEntityWithThisKeySWasFoundInTheDatabase";
    }
}