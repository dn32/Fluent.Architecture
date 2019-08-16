
using Newtonsoft.Json;

namespace Fluent.Architecture.Core.Extensions
{
    public static class JsonExtension
    {
        public static T JsonObjectToObject<T>(this object jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(jsonObject));
        }
    }
}
