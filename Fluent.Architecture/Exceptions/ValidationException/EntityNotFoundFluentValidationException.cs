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
    public class EntityNotFoundFluentValidationException : FluentValidationException
    {
        public EntityNotFoundFluentValidationException(string entityKeys)
            : base($"No entity with this key(s) was found in the database: {entityKeys}", false, entityKeys)
        {
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "NoEntityWithThisKeySWasFoundInTheDatabase";
    }
}