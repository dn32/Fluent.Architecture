using AutoMapper;
using Fluent.Architecture.Core.Extensions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Core.Util;
using Fluent.Architecture.Factory.Proxy;
using Fluent.Architecture.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.Emit;

namespace Fluent.Architecture.Core.Specifications
{
    public class FluentDynamicSpec<T> : FluentSelectSpecification<T, object> where T : FluentEntity
    {
        public string[] Fields { get; set; }

        public bool IsList { get; set; }

        public FluentDynamicSpec<T> SetParameters(string[] fields, bool isList)
        {
            Fields = fields;
            IsList = isList;
            return this;
        }

        public override IQueryable<object> Where(IQueryable<T> query)
        {
            query = query.GetInclusions(IsList);

            var descriptions = new FluentClassDescription(typeof(T), Fields);
            var rash = RandomUtil.NextRandomString(6);
            var assemblyName = $"FluentAssembly_{rash}";
            var type = CreateTypeComposition(assemblyName, descriptions);
            var types = new List<Tuple<Type, Type>>();

            GetAllTypeForDescription(descriptions, types);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(typeof(T), type).ReverseMap();
                types.ForEach(x => cfg.CreateMap(x.Item1, x.Item2).ReverseMap());
            });

            var method = typeof(AutoMapper.QueryableExtensions.Extensions).GetMethods().Where(x => x.Name == "ProjectTo").ToList()[2].MakeGenericMethod(type);
            var ret = method.Invoke(null, new object[] { query, config, null, Fields.Select(x => x).ToArray() });
            return ret as IQueryable<object>;
        }

        void GetAllTypeForDescription(FluentClassDescription description, List<Tuple<Type, Type>> list)
        {
            foreach (var property in description.Properties)
            {
                if (property.FluentClassDescription != null)
                {
                    list.Add(new Tuple<Type, Type>(property.Type, property.DynamicProperty));
                    GetAllTypeForDescription(property.FluentClassDescription, list);
                }
            }
        }

        private static Type CreateTypeComposition(string assemblyName, FluentClassDescription description)
        {
            var principaTypeBuilder = CreateTypeBuilder(assemblyName);

            foreach (var property in description.Properties)
            {
                if (property.FluentClassDescription != null)
                {
                    property.DynamicProperty = CreateTypeComposition(assemblyName, property.FluentClassDescription);
                    principaTypeBuilder.CreateProperty(property.Name, property.DynamicProperty);
                }
                else
                {
                    principaTypeBuilder.CreateProperty(property.Name, property.Type);
                }
            }

            return principaTypeBuilder.CreateType();
        }

        private static TypeBuilder CreateTypeBuilder(string assemblyName) //, IEnumerable<FluentSpecificProperty> fields)
        {
            var moduleName = "FluentCustomEntityForMap";
            var typeBuilder = BuilderClassUtil.CreateClass(typeof(object), assemblyName, moduleName);
            typeBuilder.CreateConstructor();
            return typeBuilder;
        }

        public override IOrderedQueryable<object> Order(IQueryable<object> query)
        {
            return query.OrderBy(Fields.First());
        }
    }
}
