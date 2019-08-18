
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Fluent.Architecture.Sample.Test.SupportElements
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnumPersonType
    {
        [EnumMember(Value = "None")]
        None = 0,

        [EnumMember(Value = "User")]
        User = 1,

        [EnumMember(Value = "ExternalUser")]
        ExternalUser = 2
    }
}
