using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Core.Util;
using Fluent.Architecture.Factory.Proxy;
using Fluent.Architecture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.Emit;

namespace Fluent.Architecture.Extensions
{
    public static class DynamicSpecExtension
    {
        private static string GetParameter(this Microsoft.AspNetCore.Http.HttpRequest Request, string paramName)
        {
            var value = Request.Headers[paramName].FirstOrDefault()?.Trim();
            value = string.IsNullOrWhiteSpace(value) ? Request.Query[paramName].ToString()?.Trim() : value;
            return string.IsNullOrWhiteSpace(value) ? Request.Cookies[paramName]?.Trim() : value;
        }

        private static string[] GetParameters(this Microsoft.AspNetCore.Http.HttpRequest Request, string paramName)
        {
            return Request.GetParameter(paramName)?.Split(",")?.Select(x => x?.Trim())?.Where(x => !string.IsNullOrWhiteSpace(x))?.ToArray();
        }

        public static string[] GetPropertiesToIgnore(this Microsoft.AspNetCore.Http.HttpRequest Request)
        {
            return Request.GetParameters("propertyToIgnore");
        }

        public static string[] GetPropertiesToShow(this Microsoft.AspNetCore.Http.HttpRequest Request)
        {
            return Request.GetParameters("propertyToShow");
        }

        public static string[] GetPropertiesToOrder(this Microsoft.AspNetCore.Http.HttpRequest Request)
        {
            return Request.GetParameters("propertyToOrder");
        }

        public static IQueryable<object> FluentDynamicSelectProjectTo<T>(this IQueryable<T> query, TransactionalService service) where T : BaseEntity
        {
            var Request = service.LocalHttpContext.Request;
            var fields = Request.GetPropertiesToShow();
            if (fields == null || fields.Length == 0) { return query; }
            return query.FluentDynamicSelectProjectTo(fields, out _);
        }

        public static IQueryable<T> FluentDynamicProjectTo<T>(this IQueryable<T> query, TransactionalService service, string[] fields = null) where T : BaseEntity
        {
            var Request = service.LocalHttpContext.Request;
            if (fields == null || fields.Length == 0) { fields = Request.GetPropertiesToShow(); }
            if (fields == null || fields.Length == 0) { return query; }
            return query.FluentDynamicProjectTo(fields);
        }

        public static IOrderedQueryable<T> FluentDynamicProjectToOrder<T>(this IQueryable<T> query, TransactionalService service, string[] fields = null) where T : BaseEntity
        {
            var Request = service.LocalHttpContext.Request;
            if (fields == null || fields.Length == 0) { fields = Request.GetPropertiesToShow(); }
            var order = Request.GetPropertiesToOrder();
            var orderString = order?.Length > 0 ? string.Join(",", order) : fields?.FirstOrDefault();

            if (string.IsNullOrEmpty(orderString)) { return query.OrderBy(x => x); }

            return query.OrderBy(orderString);
        }

        private static IOrderedQueryable<object> FluentDynamicProjectToSelectOrder(this IQueryable<object> query, TransactionalService service)
        {
            var Request = service.LocalHttpContext.Request;
            var show = Request.GetPropertiesToShow();
            var order = Request.GetPropertiesToOrder();
            var orderString = order?.Length > 0 ? string.Join(",", order) : show.FirstOrDefault();

            if (string.IsNullOrEmpty(orderString)) { return query.OrderBy(x => x); }

            return query.OrderBy(orderString);
        }

        private static IQueryable<T> FluentDynamicProjectTo<T>(this IQueryable<T> query, string[] Fields)
        {
            var ret = FluentDynamicSelectProjectTo(query, Fields, out MapperConfiguration config);
            return ret.ProjectTo<T>(config);
        }

        private static IQueryable<object> FluentDynamicSelectProjectTo<T>(this IQueryable<T> query, string[] Fields, out MapperConfiguration config)
        {
            var descriptions = new FluentClassDescription(typeof(T), Fields);
            var rash = RandomUtil.NextRandomString(6);
            var assemblyName = $"FluentAssembly_{rash}";
            var type = descriptions.CreateTypeComposition(assemblyName);
            var types = new List<Tuple<Type, Type>>();

            descriptions.GetAllTypeForDescription(types);

            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(typeof(T), typeof(T));
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
