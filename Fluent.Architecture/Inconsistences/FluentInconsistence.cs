
using dn32.infra.Exceptions.ValidationException;
using Newtonsoft.Json;

namespace dn32.infra.Nucleo.Inconsistences
{
    public class DnInconsistence
    {
        public string Message { get; set; }
        public string GlobalizationKey { get; set; }
        [JsonIgnore]
        public DnValidationException DnException { get; set; }
    }

    public class DnPropertyInconsistence : DnInconsistence
    {
        public string PropertyName { get; set; }
    }

    public class DnUiFieldInconsistence : DnPropertyInconsistence
    {
        public string Field { get; set; }
    }
}
