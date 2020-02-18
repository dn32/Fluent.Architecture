
using dn32.infra.Exceptions.ValidationException;
using Newtonsoft.Json;

namespace dn32.infra.Nucleo.Inconsistences
{
    public class FluenteInconsistence
    {
        public string Message { get; set; }
        public string GlobalizationKey { get; set; }
        [JsonIgnore]
        public FluenteValidationException FluenteException { get; set; }
    }

    public class FluentePropertyInconsistence : FluenteInconsistence
    {
        public string PropertyName { get; set; }
    }

    public class FluenteUiFieldInconsistence : FluentePropertyInconsistence
    {
        public string Field { get; set; }
    }
}
