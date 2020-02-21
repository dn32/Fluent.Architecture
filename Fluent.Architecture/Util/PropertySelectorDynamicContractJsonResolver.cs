using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dn32.infra.Nucleo.Util
{
    public class PropertySelectorDynamicContractJsonResolver : DefaultContractResolver
    {
        private readonly string[] PropertyToIgnore;
        private readonly string[] PropertyToShow;

        public PropertySelectorDynamicContractJsonResolver(string[] propertyToIgnore, string[] propriedadeAConsiderar)
        {
            PropertyToIgnore = propertyToIgnore;
            PropertyToShow = propriedadeAConsiderar;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            IList<JsonProperty> propertiesResult = null;

            if (PropertyToIgnore != null && PropertyToIgnore.Length > 0)
            {
                propertiesResult = properties.Where(x => !PropertyToIgnore.Contains(x.PropertyName, StringComparer.InvariantCultureIgnoreCase)).ToList();
            }

            if (PropertyToShow != null && PropertyToShow.Length > 0)
            {
                propertiesResult = properties.Where(x => PropertyToShow.Contains(x.PropertyName, StringComparer.InvariantCultureIgnoreCase)).ToList();
            }

            if (propertiesResult == null || propertiesResult.Count == 0)
            {
                return properties;
            }
            else
            {
                return propertiesResult;
            }
        }
    }
}
