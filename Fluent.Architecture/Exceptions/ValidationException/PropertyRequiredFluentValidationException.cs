// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Extensions;
using Newtonsoft.Json;
using System.Reflection;

namespace Fluent.Architecture.Exceptions.ValidationException
{
    public class PropertyRequiredFluentValidationException : FluentPropertyValidationException
    {
        public PropertyRequiredFluentValidationException(PropertyInfo propertyName) : base(propertyName.Name, false, $"The property {propertyName.Name} must have a value for this operation.")
        {
        }
    }

    public class DbFieldRequiredFluentValidationException : FluentPropertyValidationException
    {
        public DbFieldRequiredFluentValidationException(PropertyInfo propertyName) : base(propertyName.GetColumnName(), false, $"The field {propertyName.GetColumnName()} must have a value for this operation.")
        {
        }
    }

    public class DbFieldNotRequiredFluentValidationException : FluentPropertyValidationException
    {
        public DbFieldNotRequiredFluentValidationException(PropertyInfo propertyName) : base(propertyName.GetColumnName(), false, $"The {propertyName.GetColumnName()} field should not have a value for this operation.")
        {
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "ThePropertyShouldNotHaveAValueForThisOperation";
    }

    public class JsonFieldPropertyRequiredFluentValidationException : FluentPropertyValidationException
    {
        public JsonFieldPropertyRequiredFluentValidationException(PropertyInfo propertyName) : base(propertyName.GetJsonPropertyName(), false, $"The field {propertyName.GetJsonPropertyName()} must have a value for this operation.")
        {
        }
    }

    public class JsonIncorrectFieldValueFluentValidationException : FluentValidationException
    {
        public JsonIncorrectFieldValueFluentValidationException(string propertyName, string value) : base($"Field {propertyName} has an incorrect value. Value: {value}.", false, propertyName, value)
        {
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "IncorrectFieldValue";
    }

    public class UiFieldRequiredFluentValidationException : FluentPropertyValidationException
    {
        public UiFieldRequiredFluentValidationException(PropertyInfo propertyName) : base(propertyName.GetJsonPropertyName(), true, $"The field {propertyName.GetJsonPropertyName()} must have a value for this operation.")
        {
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "TheFieldMustHaveAValueForThisOperation";
    }

    public class UiFieldMaxLenghtFluentValidationException : FluentPropertyValidationException
    {
        public UiFieldMaxLenghtFluentValidationException(PropertyInfo propertyName) : base(propertyName.GetJsonPropertyName(), true, $"The {propertyName.GetJsonPropertyName()} field has more characters than allowed.")
        {
        }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "TheFieldMaxLenghtFluentValidationException";
    }
}