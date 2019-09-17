// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo
using Fluent.Architecture.Entities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluent.Architecture.Extensions
{
    /// <summary>
    /// Extensão de objetos.
    /// </summary>
    public static class ObjectExtension
    {
        public static T FluentClone<T>(this object obj1)
        {
            var json = JsonConvert.SerializeObject(obj1);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Verifica se dois objetos são iguais comparando os valores e não a referência.
        /// </summary>
        /// <param name="obj1">
        /// Primeiro objeto a ser comparado.
        /// </param>
        /// <param name="obj2">
        /// Segundo objeto a ser comparado.
        /// </param>
        public static bool CompareObjects(this object obj1, object obj2)
        {
            return obj1.GetAllDataOfObject() == obj2.GetAllDataOfObject();
        }

        public static object GetDbValue(this PropertyInfo property, object? value)
        {
            if (value == null)
            {
                return $"'{property.PropertyType.GetDefaultValue()?.ToString()?.Replace("'", "´")}'";
            }

            var type = value.GetType();

            if (type == typeof(string) || type == typeof(String))
            {
                return $"'{value?.ToString()?.Replace("'", "´")}'";
            }

            if (type == typeof(int))
            {
                return value;
            }

            if (type == typeof(Guid))
            {
                return $"'{value}'";
            }

            if (value.GetType().IsNullableEnum())
            {
                return (int)value;
            }

            return value;
        }

        /// <summary>
        /// Verifica se um objeto é nulo ou vazio
        /// </summary>
        /// <param name="value">
        /// Objeto a ser verificado.
        /// </param>
        /// <returns>
        /// Se o objeto é nulo ou vazio.
        /// </returns>
        public static bool IsFluentNull(this object? value)
        {
            if (value == null)
            {
                return true;
            }

            var type = value.GetType();

            if (type == typeof(string) || type == typeof(String))
            {
                return string.IsNullOrWhiteSpace(value.ToString());
            }

            if (type == typeof(int))
            {
                return (int)value == 0;
            }

            if (type == typeof(Guid))
            {
                return (Guid)value == Guid.Empty;
            }

            if (value.GetType().IsNullableEnum())
            {
                return (int)value == 0;
            }

            return value == value.GetType().GetDefaultValue();
        }

        /// <summary>
        /// Obtem todos os dados de um objeto, incluindo de campos e propriedades privadas.
        /// </summary>
        /// <param name="objectToCheck">Objeto a ser avaliado.</param>
        /// <returns>
        /// Json com todos os dados do onjeto.
        /// </returns>
        public static string GetAllDataOfObject(this object objectToCheck)
        {
            var propertyData = new List<NameAndValue>();
            GetAllFieldsDataOfObject(objectToCheck, propertyData);
            return JsonConvert.SerializeObject(propertyData, Formatting.None);
        }

        /// <summary>
        /// Obtem todos o nome e valor de todos os campos de um objeto.
        /// </summary>
        /// <param name="obj">Objeto a ser avaliado.</param>
        /// <param name="propertyData">
        /// Lista de valores.
        /// </param>
        /// <returns>A lista com nome e valor de todos os campos do objeto.</returns>
        private static void GetAllFieldsDataOfObject(this object obj, List<NameAndValue> propertyData)
        {
            if (obj is IQueryable)
            {
                return;
            }

            var objectType = obj.GetType();

            if (obj is ICollection collection)
            {
                foreach (var el in collection)
                {
                    if (el != null)
                    {
                        GetAllFieldsDataOfObject(el, propertyData);
                    }
                }
            }
            else
            {
                var items = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
                var type = objectType;

                while (type.Is(typeof(object)) && type != typeof(object))
                {
                    type = type.BaseType ?? type;
                    items.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
                }

                foreach (var item in items)
                {
                    if (item.FieldType.IsPrimitive || item.FieldType.IsValueType || item.FieldType == typeof(string))
                    {
                        var data = item.GetValue(obj);
                        if (data != null)
                        {
                            propertyData.Add(new NameAndValue { Name = item.Name.Replace("k__BackingField", string.Empty, StringComparison.OrdinalIgnoreCase) ?? "", Value = data });
                        }
                    }
                    else if (item.FieldType.IsClass && !typeof(IEnumerable).IsAssignableFrom(item.FieldType))
                    {
                        var data = item.GetValue(obj);
                        if (data != null)
                        {
                            GetAllFieldsDataOfObject(data, propertyData);
                        }
                    }
                    else
                    {
                        if (!(item.GetValue(obj) is IEnumerable enumerablePropObj1))
                        {
                            continue;
                        }

                        foreach (var propItem in enumerablePropObj1)
                        {
                            if (propItem != null)
                            {
                                GetAllFieldsDataOfObject(propItem, propertyData);
                            }
                        }
                    }
                }
            }
        }
    }
}