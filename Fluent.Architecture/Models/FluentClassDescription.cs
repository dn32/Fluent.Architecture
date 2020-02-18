using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace dn32.infra.Nucleo.Models
{
    internal class DnClassDescription
    {
        public DnClassDescription(Type principalType, string[] fields)
        {
            Properties = new List<DnPropertyDescription>();

            var compositions = fields.OrderBy(x => x).Where(x => x.Contains(".")).ToList();
            var anotherProperties = fields.OrderBy(x => x).Where(x => !x.Contains(".")).ToList();

            foreach (var property in anotherProperties)
            {
                var complexProperty = GetSimpleProperty(principalType, property);
                Properties.Add(complexProperty);
            }

            foreach (var property in compositions)
            {
                AddCompositionProperty(Properties, principalType, property);
            }
        }

        private static void AddCompositionProperty(List<DnPropertyDescription> Properties, Type principalType, string property)
        {
            var index = property.IndexOf(".");
            var className = property.Substring(0, index);
            var propertyName = property.Substring(index + 1);
            var propertyInfo = principalType.GetProperty(className) ?? throw new InvalidOperationException($"Entidade {principalType.Name} does not have property {className}");
            var complexPropertyFound = Properties.SingleOrDefault(x => x.Name == className);

            if (complexPropertyFound == null)
            {
                var complexProperty = new DnPropertyDescription { Name = className, Type = propertyInfo.PropertyType, DnClassDescription = new DnClassDescription(propertyInfo.PropertyType, new[] { propertyName }) };
                Properties.Add(complexProperty);
            }
            else
            {
                if (propertyName.Contains("."))
                {
                    AddCompositionProperty(complexPropertyFound.DnClassDescription.Properties, propertyInfo.PropertyType, propertyName);
                }
                else
                {
                    var simpleProperty = GetSimpleProperty(propertyInfo.PropertyType, propertyName);
                    complexPropertyFound.DnClassDescription.Properties.Add(simpleProperty);
                }
            }
        }

        private static DnPropertyDescription GetSimpleProperty(Type principalType, string property)
        {
            var propertyInfo = principalType.GetProperties().FirstOrDefault(x => x.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase)) ?? throw new InvalidOperationException($"Entidade {principalType.Name} does not have property {property}");
            return new DnPropertyDescription { Name = property, Type = propertyInfo.PropertyType };
        }

        public List<DnPropertyDescription> Properties { get; set; }
    }
}
