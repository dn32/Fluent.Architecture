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
    public class FluentUiFieldValidationException : FluentPropertyValidationException
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "FluentUiFieldValidationException";

        public FluentUiFieldValidationException(PropertyInfo property, bool globalizeValues, string message) : base(property, globalizeValues, message)
        {
            Field = property.GetUiPropertyName();
        }
    }

    public class UiFieldRequiredFluentValidationException : FluentUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "TheFieldMustHaveAValueForThisOperation";

        public UiFieldRequiredFluentValidationException(PropertyInfo property) : base(property, true, $"The field {property.GetUiPropertyName()} must have a value for this operation.")
        {
        }
    }

    public class UiFieldLenghtFluentValidationException : FluentUiFieldValidationException
    {
        [JsonProperty("globalization_key")]
        public override string GlobalizationKey => "TheFieldLenghtFluentValidationException";

        public int Min { get; set; }

        public int Max { get; set; }

        public UiFieldLenghtFluentValidationException(PropertyInfo property) : base(property, true, $"The {property.GetUiPropertyName()} field has more or less characters than allowed.")
        {
            var ret = property.GetPropertyRange();
            Min = ret?.min ?? 0;
            Max = ret?.max ?? 0;

            Field = property.GetUiPropertyName();
        }
    }

    //public class DbFieldRequiredFluentValidationException : FluentPropertyValidationException
    //{
    //    public DbFieldRequiredFluentValidationException(PropertyInfo propertyName) : base(propertyName.GetColumnName(), false, $"The field {propertyName.GetColumnName()} must have a value for this operation.")
    //    {
    //    }
    //}

    //public class PropertyRequiredFluentValidationException : FluentPropertyValidationException
    //{
    //    public PropertyRequiredFluentValidationException(PropertyInfo propertyName) : base(propertyName?.Name, false, $"The property {propertyName.Name} must have a value for this operation.")
    //    {
    //    }
    //}

    //public class DbFieldNotRequiredFluentValidationException : FluentPropertyValidationException
    //{
    //    public DbFieldNotRequiredFluentValidationException(PropertyInfo propertyName) : base(propertyName.GetColumnName(), false, $"The {propertyName.GetColumnName()} field should not have a value for this operation.")
    //    {
    //    }

    //    [JsonProperty("globalization_key")]
    //    public override string GlobalizationKey => "ThePropertyShouldNotHaveAValueForThisOperation";
    //}

    //public class JsonFieldPropertyRequiredFluentValidationException : FluentPropertyValidationException
    //{
    //    public JsonFieldPropertyRequiredFluentValidationException(PropertyInfo propertyName) : base(propertyName.GetJsonPropertyName(), false, $"The field {propertyName.GetJsonPropertyName()} must have a value for this operation.")
    //    {
    //    }
    //}

    //public class JsonIncorrectFieldValueFluentValidationException : FluentValidationException
    //{
    //    public JsonIncorrectFieldValueFluentValidationException(string propertyName, string value) : base($"Field {propertyName} has an incorrect value. Value: {value}.", false, propertyName, value)
    //    {
    //    }

    //    [JsonProperty("globalization_key")]
    //    public override string GlobalizationKey => "IncorrectFieldValue";
    //}


}