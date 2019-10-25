using AutoMapper;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Core.Util;
using Fluent.Architecture.Factory.Proxy;
using Fluent.Architecture.Services;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.Emit;

namespace Fluent.Architecture.Core.Specifications
{
    public static class DynamicSpecExtension
    {
        public static string[] GetPropertiesToIgnore(this Microsoft.AspNetCore.Http.HttpRequest Request)
        {
            Request.Headers.TryGetValue("propertyToIgnore", out StringValues properties);
            var propertiesList = properties.ToList().Select(x => x.Split(",")).SelectMany(x => x).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return propertiesList;
        }

        public static string[] GetPropertiesToShow(this Microsoft.AspNetCore.Http.HttpRequest Request)
        {
            Request.Headers.TryGetValue("propertyToShow", out StringValues properties);
            var propertiesList = properties.ToList().Select(x => x.Split(",")).SelectMany(x => x).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return propertiesList;
        }

        public static string[] GetPropertiesToOrder(this Microsoft.AspNetCore.Http.HttpRequest Request)
        {
            Request.Headers.TryGetValue("propertyToOrder", out StringValues properties);
            var propertiesList = properties.ToList().Select(x => x.Split(",")).SelectMany(x => x).Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return propertiesList;
        }

        public static IQueryable<object> FluentDynamicProjectTo<T>(this IQueryable<T> query, TransactionalService service) where T : BaseEntity
        {
            var Request = service.LocalHttpContext.Request;
            var show = Request.GetPropertiesToShow();
            if (show.Length == 0) { return query; }
            return query.FluentDynamicProjectTo(show);
        }

        public static IOrderedQueryable<object> FluentDynamicProjectToOrder(this IQueryable<object> query, TransactionalService service)
        {
            var Request = service.LocalHttpContext.Request;
            var show = Request.GetPropertiesToShow();
            var order = Request.GetPropertiesToOrder();
            var orderString = order.Length > 0 ? string.Join(",", order) : show.FirstOrDefault();

            if (string.IsNullOrEmpty(orderString)) { return query.OrderBy(x => x); }

            return query.OrderBy(orderString);
        }

        public static IQueryable<object> FluentDynamicProjectTo<T>(this IQueryable<T> query, string[] Fields)
        {
            var descriptions = new FluentClassDescription(typeof(T), Fields);
            var rash = RandomUtil.NextRandomString(6);
            var assemblyName = $"FluentAssembly_{rash}";
            var type = descriptions.CreateTypeComposition(assemblyName);
            var types = new List<Tuple<Type, Type>>();

            descriptions.GetAllTypeForDescription(types);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(typeof(T), type).ReverseMap();
                types.ForEach(x => cfg.CreateMap(x.Item1, x.Item2).ReverseMap());
            });

            var method = typeof(AutoMapper.QueryableExtensions.Extensions).GetMethods().Where(x => x.Name == "ProjectTo").ToList()[2].MakeGenericMethod(type);
            var ret = method.Invoke(null, new object[] { query, config, null, Fields.Select(x => x).ToArray() });
            return ret as IQueryable<object>;
        }

        internal static void GetAllTypeForDescription(this FluentClassDescription description, List<Tuple<Type, Type>> list)
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

        internal static Type CreateTypeComposition(this FluentClassDescription description, string assemblyName)
        {
            var principaTypeBuilder = CreateTypeBuilder(assemblyName);

            foreach (var property in description.Properties)
            {
                if (property.FluentClassDescription != null)
                {
                    property.DynamicProperty = CreateTypeComposition(property.FluentClassDescription, assemblyName);
                    principaTypeBuilder.CreateProperty(property.Name, property.DynamicProperty);
                }
                else
                {
                    principaTypeBuilder.CreateProperty(property.Name, property.Type);
                }
            }

            return principaTypeBuilder.CreateType();
        }

        internal static TypeBuilder CreateTypeBuilder(string assemblyName)
        {
            var moduleName = "FluentCustomEntityForMap";
            var typeBuilder = BuilderClassUtil.CreateClass(typeof(object), assemblyName, moduleName);
            typeBuilder.CreateConstructor();
            return typeBuilder;
        }
    }
}
