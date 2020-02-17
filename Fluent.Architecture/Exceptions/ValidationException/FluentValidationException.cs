// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluente.Arquitetura.Util;
using Newtonsoft.Json;
using System.Threading;

namespace Fluente.Arquitetura.Exceptions.ValidationException
{
    /// <inheritdoc />
    public class FluenteValidationException
    {
        private readonly string globalizationKey;

#pragma warning disable CA1822
        [JsonProperty("validation_error")]
        public bool ValidationError => true;
#pragma warning restore CA1822

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

        public FluenteValidationException(string message, string globalizationKeyParam, bool globalizeValues = false, params string[] values)
        {
            globalizationKey = globalizationKeyParam;
            Message = message;
            Values = values;
            GlobalizeValues = globalizeValues;
            //Inicialize(message);
        }

        public FluenteValidationException(string message, bool globalizeValues = false, params string[] values)
        {
            Message = message;
            Values = values;
            GlobalizeValues = globalizeValues;
            //Inicialize(message);
        }

        //private void Inicialize(string message)
        //{
        //    //if (Setup.GlobalizationService != null)
        //    //{
        //    //    if (GlobalizeValues)
        //    //    {
        //    //        for (var i = 0; i < Values.Length; i++)
        //    //        {
        //    //            Values[i] = Setup.GlobalizationService.GetResource("campo_" + Values[i], Values[i]);
        //    //        }
        //    //    }

        //    //    GlobalizedMessage = Setup.GlobalizationService.GetResource(GlobalizationKey, message, Values);
        //    //}
        //}
    }
}