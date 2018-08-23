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
        public static object GetDbValue(this object value)
        {
            if (value == null)
            {
                return null;
            }

            var type = value.GetType();

            if (type == typeof(string))
            {
                return $"'{value}'";
            }
            else if (type == typeof(int))
            {
                return value;

            }
            else if (type == typeof(Guid))
            {
                return $"'{value}'";
            }

            try
            {
                var valorInt = (int)value; // Enum
                return valorInt;
            }
            catch (System.Exception)
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

            if (type == typeof(string))
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return true;
                }
            }
            else if (type == typeof(int))
            {
                if ((int)value == 0)
                {
                    return true;
                }
            }
            else if (type == typeof(Guid))
            {
                if ((Guid)value == Guid.Empty)
                {
                    return true;
                }
            }

            try
            {
                var valorInt = (int)value; // Enum
                if (valorInt == 0)
                {
                    return true;
                }
            }
            catch (System.Exception)
            {
                // ignored
            }

            return false;
        }

        /// <summary>
        /// Obtem todos os dados de um objeto, incluindo de campos e propriedades privadas.
        /// </summary>
        /// <param name="obj">Objeto a ser avaliado.</param>
        /// <returns>Json com todos os dados do onjeto. Exemplo: [{"Name":"Id","Value":0},{"Name":"DataFinal","Value":"9999-12-31T23:59:59.9999999"},{"Name":"IdiomaId","Value":1},{"Name":"ConteudoId","Value":1},{"Name":"TipoDeEvento","Value":"2"},{"Name":"Descricao","Value":"teste"},{"Name":"Mandatorio","Value":1},{"Name":"UsuarioId","Value":1},{"Name":"TipoDeConteudo","Value":"1"}][{"Name":"<Id>k__BackingField","Value":0},{"Name":"<DataFinal>k__BackingField","Value":"9999-12-31T23:59:59.9999999"},{"Name":"<IdiomaId>k__BackingField","Value":1},{"Name":"<ConteudoId>k__BackingField","Value":1},{"Name":"<TipoDeEvento>k__BackingField","Value":"2"},{"Name":"<Descricao>k__BackingField","Value":"teste"},{"Name":"<Mandatorio>k__BackingField","Value":1},{"Name":"<UsuarioId>k__BackingField","Value":1},{"Name":"<TipoDeConteudo>k__BackingField","Value":"1"}]</returns>
        public static string GetAllDataOfObject(this object obj)
        {
            var contentproperty = GetAllPropertyDataOfObject(obj);
            var contentFields = GetAllFieldsDataOfObject(obj);
            return JsonConvert.SerializeObject(contentproperty, Formatting.None) + JsonConvert.SerializeObject(contentFields, Formatting.None);
        }

        /// <summary>
        /// Obtem todos o nome e valor de todos os campos de um objeto.
        /// </summary>
        /// <param name="obj">Objeto a ser avaliado.</param>
        /// <returns>A lista com nome e valor de todos os campos do objeto.</returns>
        public static List<NameAndValue> GetAllFieldsDataOfObject(this object obj)
        {
            if (obj == null || obj is IQueryable)
            {
                return new List<NameAndValue>();
            }

            var propertyInformations = new List<NameAndValue>();


            if (obj is ICollection colection)
            {
                foreach (var el in colection)
                {
                    propertyInformations.AddRange(GetAllFieldsDataOfObject(el));
                }
            }
            else
            {

                foreach (var item in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    //for value types
                    if (item.FieldType.IsPrimitive || item.FieldType.IsValueType || item.FieldType == typeof(string))
                    {
                        propertyInformations.Add(new NameAndValue { Name = item.Name, Value = item.GetValue(obj) });
                    }
                    //for complex types
                    else if (item.FieldType.IsClass && !typeof(IEnumerable).IsAssignableFrom(item.FieldType))
                    {
                        propertyInformations.AddRange(GetAllFieldsDataOfObject(item.GetValue(obj)));
                        propertyInformations.AddRange(GetAllPropertyDataOfObject(item.GetValue(obj)));
                    }
                    //for Enumerables
                    else
                    {
                        var enumerablePropObj1 = item.GetValue(obj) as IEnumerable;

                        if (enumerablePropObj1 == null) continue;

                        var objList = enumerablePropObj1.GetEnumerator();

                        while (objList.MoveNext())
                        {
                            objList.MoveNext();
                            GetAllFieldsDataOfObject(objList.Current);
                            GetAllPropertyDataOfObject(objList.Current);
                        }
                    }
                }
            }

            return propertyInformations;
        }


        /// <summary>
        /// Obtem todos o nome e valor de todas as propriedades de um objeto.
        /// </summary>
        /// <param name="obj">Objeto a ser avaliado.</param>
        /// <returns>A lista com nome e valor de todas as propriedades do objeto.</returns>
        public static List<NameAndValue> GetAllPropertyDataOfObject(this object obj)
        {
            if (obj == null || obj is IQueryable)
            {
                return new List<NameAndValue>();
            }

            var propertyInformations = new List<NameAndValue>();

            if (obj is ICollection colection)
            {
                foreach (var el in colection)
                {
                    propertyInformations.AddRange(GetAllFieldsDataOfObject(el));
                }
            }
            else
            {
                foreach (var item in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    //for value types
                    if (item.PropertyType.IsPrimitive || item.PropertyType.IsValueType || item.PropertyType == typeof(string))
                    {
                        propertyInformations.Add(new NameAndValue { Name = item.Name, Value = item.GetValue(obj) });
                    }
                    //for complex types
                    else if (item.PropertyType.IsClass && !typeof(IEnumerable).IsAssignableFrom(item.PropertyType))
                    {
                        propertyInformations.AddRange(GetAllPropertyDataOfObject(item.GetValue(obj)));
                        propertyInformations.AddRange(GetAllFieldsDataOfObject(item.GetValue(obj)));
                    }
                    //for Enumerables
                    else
                    {
                        var enumerablePropObj1 = item.GetValue(obj) as IEnumerable;

                        if (enumerablePropObj1 == null) continue;

                        var objList = enumerablePropObj1.GetEnumerator();

                        while (objList.MoveNext())
                        {
                            objList.MoveNext();
                            GetAllPropertyDataOfObject(objList.Current);
                            GetAllFieldsDataOfObject(objList.Current);
                        }
                    }
                }
            }

            return propertyInformations;
        }
    }
}