using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Fluent.Architecture.Core.Models
{
    public class FluentClassDescription
    {
        public FluentClassDescription(Type principalType, string[] fields)
        {
            Properties = new List<FluentPropertyDescription>();

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

        private static void AddCompositionProperty(List<FluentPropertyDescription> Properties, Type principalType, string property)
        {
            var index = property.IndexOf(".");
            var className = property.Substring(0, index);
            var propertyName = property.Substring(index + 1);
            var propertyInfo = principalType.GetProperty(className) ?? throw new InvalidOperationException($"Entity {principalType.Name} does not have property {className}");
            var complexPropertyFound = Properties.SingleOrDefault(x => x.Name == className);

            if (complexPropertyFound == null)
            {
                var complexProperty = new FluentPropertyDescription { Name = className, Type = propertyInfo.PropertyType, FluentClassDescription = new FluentClassDescription(propertyInfo.PropertyType, new[] { propertyName }) };
                Properties.Add(complexProperty);
            }
            else
            {
                if (propertyName.Contains("."))
                {
                    AddCompositionProperty(complexPropertyFound.FluentClassDescription.Properties, propertyInfo.PropertyType, propertyName);
                }
                else
                {
                    var simpleProperty = GetSimpleProperty(propertyInfo.PropertyType, propertyName);
                    complexPropertyFound.FluentClassDescription.Properties.Add(simpleProperty);
                }
            }
        }

        private static FluentPropertyDescription GetSimpleProperty(Type principalType, string property)
        {
            var propertyInfo = principalType.GetProperty(property) ?? throw new InvalidOperationException($"Entity {principalType.Name} does not have property {property}");
            return new FluentPropertyDescription { Name = property, Type = propertyInfo.PropertyType };
        }

        public List<FluentPropertyDescription> Properties { get; set; }
    }
}
