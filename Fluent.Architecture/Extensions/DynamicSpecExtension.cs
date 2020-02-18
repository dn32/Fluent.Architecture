using AutoMapper;
using AutoMapper.QueryableExtensions;
using dn32.infra.Factory.Proxy;
using dn32.infra.Nucleo.Models;
using dn32.infra.Nucleo.Util;
using dn32.infra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection.Emit;
using dn32.infra.dados;

namespace dn32.infra.Extensoes
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

        public static IQueryable<object> DnDynamicSelectProjectTo<T>(this IQueryable<T> query, TransactionalService service) where T : EntidadeBase
        {
            var Request = service.LocalHttpContext.Request;
            var fields = Request.GetPropertiesToShow();
            if (fields == null || fields.Length == 0) { return query; }
            return query.DnDynamicSelectProjectTo(fields, out _);
        }

        public static IQueryable<T> DnDynamicProjectTo<T>(this IQueryable<T> query, TransactionalService service, string[] fields = null) where T : EntidadeBase
        {
            var Request = service.LocalHttpContext.Request;
            if (fields == null || fields.Length == 0) { fields = Request.GetPropertiesToShow(); }
            if (fields == null || fields.Length == 0) { return query; }
            return query.DnDynamicProjectTo(fields);
        }

        public static IOrderedQueryable<T> DnDynamicProjectToOrder<T>(this IQueryable<T> query, TransactionalService service, string[] fields = null) where T : EntidadeBase
        {
            var Request = service.LocalHttpContext.Request;
            if (fields == null || fields.Length == 0) { fields = Request.GetPropertiesToShow(); }
            var order = Request.GetPropertiesToOrder();
            var orderString = order?.Length > 0 ? string.Join(",", order) : fields?.FirstOrDefault();

            if (string.IsNullOrEmpty(orderString)) { return query.OrderBy(x => x); }

            return query.OrderBy(orderString);
        }

        private static IOrderedQueryable<object> DnDynamicProjectToSelectOrder(this IQueryable<object> query, TransactionalService service)
        {
            var Request = service.LocalHttpContext.Request;
            var show = Request.GetPropertiesToShow();
            var order = Request.GetPropertiesToOrder();
            var orderString = order?.Length > 0 ? string.Join(",", order) : show.FirstOrDefault();

            if (string.IsNullOrEmpty(orderString)) { return query.OrderBy(x => x); }

            return query.OrderBy(orderString);
        }

        private static IQueryable<T> DnDynamicProjectTo<T>(this IQueryable<T> query, string[] Fields)
        {
            var ret = DnDynamicSelectProjectTo(query, Fields, out MapperConfiguration config);
            return ret.ProjectTo<T>(config);
        }

        private static IQueryable<object> DnDynamicSelectProjectTo<T>(this IQueryable<T> query, string[] Fields, out MapperConfiguration config)
        {
            var descriptions = new DnClassDescription(typeof(T), Fields);
            var rash = RandomUtil.NextRandomString(6);
            var assemblyName = $"DnAssembly_{rash}";
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

        internal static void GetAllTypeForDescription(this DnClassDescription description, List<Tuple<Type, Type>> list)
        {
            foreach (var property in description.Properties)
            {
                if (property.DnClassDescription != null)
                {
                    list.Add(new Tuple<Type, Type>(property.Type, property.DynamicProperty));
                    GetAllTypeForDescription(property.DnClassDescription, list);
                }
            }
        }

        internal static Type CreateTypeComposition(this DnClassDescription description, string assemblyName)
        {
            var principaTypeBuilder = CreateTypeBuilder(assemblyName);

            foreach (var property in description.Properties)
            {
                if (property.DnClassDescription != null)
                {
                    property.DynamicProperty = CreateTypeComposition(property.DnClassDescription, assemblyName);
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
            var moduleName = "DnCustomEntityForMap";
            var typeBuilder = BuilderClassUtil.CreateClass(typeof(object), assemblyName, moduleName);
            typeBuilder.CreateConstructor();
            return typeBuilder;
        }
    }
}
