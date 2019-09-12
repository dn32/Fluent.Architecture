using Newtonsoft.Json;
using System;
using System.Linq;

namespace Fluent.Architecture.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FluentReferenceAttribute : Attribute
    {
        public string[] LocalKeys { get; set; }

        public string[] ExternalKeys { get; set; }

        [JsonIgnore]
        public string LocalKey
        {
            get
            {
                return ExternalKeys.First();
            }
            set
            {
                LocalKeys = new[] { value };
            }
        }

        [JsonIgnore]
        public string ExternalKey
        {
            get
            {
                return ExternalKeys.First();
            }
            set
            {
                ExternalKeys = new[] { value };
            }
        }

        public void SetType(string type) => Type = type;

        public void SetName(string propertyName) => PropertyName = propertyName;

        public string Type { get; private set; }

        public string PropertyName { get; private set; }

        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}
