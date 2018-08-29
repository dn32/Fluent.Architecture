// ReSharper disable CommentTypo
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fluent.Architecture.Model;
using Newtonsoft.Json;

namespace Fluent.Architecture.Extensions
{
    /// <summary>
    /// Extensão de objetos.
    /// </summary>
    public static class ObjectExtension
    {
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

        public static object GetDbValue(this object value, PropertyInfo property = null)
        {
            if (value == null)
            {
                if (property != null)
                {
                    return $"'{property.PropertyType.GetDefaultValue()}'";
                }

                return null;
            }

            var type = value.GetType();

            if (type == typeof(string) || type == typeof(String))
            {
                return $"'{value}'";
            }

            if (type == typeof(int))
            {
                return value;
            }

            if (type == typeof(Guid))
            {
                return $"'{value}'";
            }

            try
            {
                return (int)value; // Enum
            }
            catch (Exception)
            {
                // ignored
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
        public static bool IsFluentNull(this object value)
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

            try
            {
                return (int)value == 0; // Enum
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }

        /// <summary>
        /// Obtem todos os dados de um objeto, incluindo de campos e propriedades privadas.
        /// </summary>
        /// <param name="objectToCheck">Objeto a ser avaliado.</param>
        /// <returns>Json com todos os dados do onjeto. Exemplo: [{"Name":"Id","Value":0},{"Name":"DataFinal","Value":"9999-12-31T23:59:59.9999999"},{"Name":"IdiomaId","Value":1},{"Name":"ConteudoId","Value":1},{"Name":"TipoDeEvento","Value":"2"},{"Name":"Descricao","Value":"teste"},{"Name":"Mandatorio","Value":1},{"Name":"UsuarioId","Value":1},{"Name":"TipoDeConteudo","Value":"1"}][{"Name":"<Id>k__BackingField","Value":0},{"Name":"<DataFinal>k__BackingField","Value":"9999-12-31T23:59:59.9999999"},{"Name":"<IdiomaId>k__BackingField","Value":1},{"Name":"<ConteudoId>k__BackingField","Value":1},{"Name":"<TipoDeEvento>k__BackingField","Value":"2"},{"Name":"<Descricao>k__BackingField","Value":"teste"},{"Name":"<Mandatorio>k__BackingField","Value":1},{"Name":"<UsuarioId>k__BackingField","Value":1},{"Name":"<TipoDeConteudo>k__BackingField","Value":"1"}]</returns>
        /// <returns>
        /// O json referente ao objeto.
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
            if (obj == null || obj is IQueryable)
            {
                return;
            }

            var objectType = obj.GetType();

            if (obj is ICollection collection)
            {
                foreach (var el in collection)
                {
                    GetAllFieldsDataOfObject(el, propertyData);
                }
            }
            else
            {
                foreach (var item in objectType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (item.FieldType.IsPrimitive || item.FieldType.IsValueType || item.FieldType == typeof(string))
                    {
                        propertyData.Add(new NameAndValue { Name = item.Name.Replace("k__BackingField", string.Empty), Value = item.GetValue(obj) });
                    }
                    else if (item.FieldType.IsClass && !typeof(IEnumerable).IsAssignableFrom(item.FieldType))
                    {
                        GetAllFieldsDataOfObject(item.GetValue(obj), propertyData);
                    }
                    else
                    {
                        if (!(item.GetValue(obj) is IEnumerable enumerablePropObj1))
                        {
                            continue;
                        }

                        foreach (var propItem in enumerablePropObj1)
                        {
                            GetAllFieldsDataOfObject(propItem, propertyData);
                        }
                    }
                }
            }
        }
    }
}