using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Extensions;
using Newtonsoft.Json;

namespace Fluent.Architecture.Core.Models
{
    //Todo - 001 Testar
    public abstract class BaseEntity
    {
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();

        public override int GetHashCode() => GetHasCodeByKeyProperties();

        private int GetHasCodeByKeyProperties()
        {
            var type = GetType();
            var json = GetKeyValuesString(type);
            return json.GetHashCode();
        }

        private string GetKeyValuesString(Type type) => type.GetHashCode() + JsonConvert.SerializeObject(GetAllPropertiesValue(type));

        private object[] GetAllPropertiesValue(Type type)
        {
            var keyElements = GetAllKeyProperties(type);
            return keyElements.Select(x => x.GetValue(this)).ToArray();
        }

        private IEnumerable<PropertyInfo> GetAllKeyProperties(Type type) => type.GetProperties().Where(x => x.GetCustomAttributeAny<KeyAttribute>());
    }
}
