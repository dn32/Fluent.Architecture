// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Util;
using Newtonsoft.Json;
using System.Threading;

namespace Fluent.Architecture.Exceptions.ValidationException
{
    /// <inheritdoc />
    public class FluentValidationException
    {
        protected string globalizationKey { get; set; }

        [JsonProperty("validation_error")]
        public bool ValidationError => true;

        [JsonProperty("message")]
        public virtual string Message { get; set; }

        [JsonProperty("globalization_key")]
        public virtual string GlobalizationKey => string.IsNullOrEmpty(globalizationKey) ? Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Message.ToLower()).ClearText() : globalizationKey;

        [JsonProperty("globalized_message")]
        public virtual string GlobalizedMessage { get; set; }

        [JsonProperty("values")]
        public virtual string[] Values { get; set; }

        [JsonProperty("globalize_values")]
        public bool GlobalizeValues { get; }

        [JsonProperty("exception_type")]
        public string ExceptionType => GetType().Name;

        public FluentValidationException(string message, string _globalizationKey, bool globalizeValues = false, params string[] values)
        {
            globalizationKey = _globalizationKey;
            Message = message;
            Values = values;
            GlobalizeValues = globalizeValues;
            Inicialize(message);
        }

        public FluentValidationException(string message, bool globalizeValues = false, params string[] values)
        {
            Message = message;
            Values = values;
            GlobalizeValues = globalizeValues;

            Inicialize(message);
        }

        private void Inicialize(string message)
        {
            if (Setup.GlobalizationService != null)
            {
                if (GlobalizeValues)
                {
                    for (var i = 0; i < Values.Length; i++)
                    {
                        Values[i] = Setup.GlobalizationService.GetResource("campo_" + Values[i], Values[i]);
                    }
                }

                GlobalizedMessage = Setup.GlobalizationService.GetResource(GlobalizationKey, message, Values);
            }
        }
    }
}