
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Extensions;
using Newtonsoft.Json;
using System.Reflection;

namespace Fluent.Architecture.Core.Inconsistences
{
    public class FluentInconsistence
    {
        public string Message { get; set; }
        public string GlobalizationKey { get; set; }
        [JsonIgnore]
        public FluentValidationException FluentException { get; set; }
    }

    public class FluentPropertyInconsistence : FluentInconsistence
    {
        public string PropertyName => Property.GetJsonPropertyName();

        [JsonIgnore]
        public PropertyInfo Property { get; set; }
    }

    public class FluentUiFieldInconsistence : FluentPropertyInconsistence
    {
        public string Field { get; set; }
    }
}
