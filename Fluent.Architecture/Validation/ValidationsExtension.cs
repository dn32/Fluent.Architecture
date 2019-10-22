// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable CommentTypo

using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Core.Models;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Fluent.Architecture.Validation
{
    internal static class ValidationsExtension
    {
        /*
         === PADRÃO DE NOMECLATURA ===
         O que deve ser verdadeiro
         ParameterMustBeInformed 
         (O parâmetro deve ser informado. Se não for informado, teremos uma inconsistência)
         Evite escrever negação, mas quando não for possível evitar, escreva assim: EntityShouldNotExistInDatabase.
         A entida não pode existir. Se existir, teremos uma inconsistência.
         =============================         
        */

        internal static void FluentValidateAttribute<T>(this IFluentValidation validation, T entity, string compositionProperty, string compositionFieldName) where T : BaseEntity
        {
            if (!validation.NullParameterOk)
            {
                return;
            }

            var properties = entity.GetType().GetProperties();
            foreach (var property in properties)
            {
                var FluentValidateAttribute = property.GetCustomAttribute<FluentValidateAttribute>(true)?.FluentCast<FluentValidateAttribute>();
                if (FluentValidateAttribute == null) { continue; }

                FluentValidateAttribute.Entity = entity;
                var value = property.GetValue(entity);
                if (!FluentValidateAttribute.IsValidWhen(value))
                {
                    validation.AddInconsistency(new FluentGenericAttributeValidateException(property, false, FluentValidateAttribute.InvalidMessage, compositionProperty, compositionFieldName));
                }
            }
        }

        internal static void ParameterMustBeInformed(this IFluentValidation validation, object obj, string compositionProperty)
        {
            if (obj == null)
            {
                validation.AddInconsistency(new NullParameterFluentValidationException(compositionProperty ?? nameof(obj)));
                validation.NullParameterOk = false;
                return;
            }

            validation.NullParameterOk = true;
        }

        internal static void MaxMinLenghtPropertyMustBeInformed<T>(this IFluentValidation validation, T entity, string compositionProperty, string compositionFieldName) where T : BaseEntity
        {
            if (!validation.NullParameterOk)
            {
                return;
            }

            var properties = entity.GetType().GetProperties().ToList();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributeAny<FluentRandomKeyValueOnAddAttribute>(true)) { continue; }
                if (!string.IsNullOrWhiteSpace(compositionProperty))
                {
                    if (property.GetCustomAttribute<FluentJsonPropertyAttribute>(true)?.form == Core.Enumerator.EnumForm.HIDDEN)
                    {
                        continue;
                    }
                }
                var value = property.GetValue(entity);
                if (value == null) { continue; }
                if (property.PropertyType.IsNumeric())
                {
                    var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<RangeAttribute>()?.Minimum;
                    var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<RangeAttribute>()?.Maximum;
                    var mindouble = min == null ? double.MinValue : double.Parse(min.ToString(), CultureInfo.InvariantCulture);
                    var maxdouble = max == null ? double.MaxValue : double.Parse(max.ToString(), CultureInfo.InvariantCulture);
                    var stringValue = value.ToString();
                    if (stringValue.Contains(".")) { stringValue = stringValue.Split(".")[0]; }
                    if (stringValue.Contains(",")) { stringValue = stringValue.Split(",")[0]; }
                    var valuedoble = double.Parse(stringValue, CultureInfo.InvariantCulture);
                    if (valuedoble < mindouble || valuedoble > maxdouble)
                    {
                        validation.AddInconsistency(new UiFieldLenghtFluentValidationException(property, compositionProperty, compositionFieldName));
                    }
                }

                if (property.PropertyType == typeof(string) && property.PropertyType == typeof(String))
                {
                    var requ = property.GetCustomAttribute<RequiredAttribute>() != null || property.GetCustomAttribute<FluentRequiredAttribute>() != null;
                    if (requ && property.GetValue(entity).IsFluentNull())
                    {//Nesse caso já há uma inconsistência de requerido adicionada
                        return;
                    }

                    var min = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.min ?? property.GetCustomAttribute<MinLengthAttribute>()?.Length;
                    var max = property.GetCustomAttribute<FluentJsonPropertyAttribute>()?.max ?? property.GetCustomAttribute<MaxLengthAttribute>()?.Length;
                    if (min == null || max == null) { continue; }

                    if (!new MinLengthAttribute(min.Value).IsValid(value))
                    {
                        validation.AddInconsistency(new UiFieldLenghtFluentValidationException(property, compositionProperty, compositionFieldName));
                    }

                    var maxint = Convert.ChangeType(max, typeof(int), CultureInfo.InvariantCulture) as int?;
                    maxint = maxint == 0 ? int.MaxValue : maxint;
                    if (!new MaxLengthAttribute(maxint.Value).IsValid(value))
                    {
                        validation.AddInconsistency(new UiFieldLenghtFluentValidationException(property, compositionProperty, compositionFieldName));
                    }
                }
            }
        }

        internal static void RequiredPropertyMustBeInformed<T>(this IFluentValidation validation, T entity, string compositionProperty, string compositionFieldName) where T : BaseEntity
        {
            if (!validation.NullParameterOk)
            {
                return;
            }

            var properties = typeof(T).GetPropertiesByAttribute<RequiredAttribute>();
            var properties2 = typeof(T).GetPropertiesByAttribute<FluentRequiredAttribute>();

            properties2.ForEach(x =>
            {
                if (!properties.Contains(x))
                {
                    properties.Add(x);
                }
            });

            foreach (var property in properties)
            {
                if (property.GetCustomAttributeAny<FluentRandomKeyValueOnAddAttribute>(true)) { continue; }
                if (!string.IsNullOrWhiteSpace(compositionProperty))
                {
                    if (property.GetCustomAttribute<FluentJsonPropertyAttribute>(true)?.form == Core.Enumerator.EnumForm.HIDDEN)
                    {
                        continue;
                    }
                }
                if (property.GetValue(entity).IsFluentNull())
                {
                    validation.AddInconsistency(new UiFieldRequiredFluentValidationException(property, compositionProperty, compositionFieldName));
                }
            }
        }

        internal static void AllKeysMustBeInformed<T>(this IFluentValidation validation, T entity, string compositionProperty, string compositionFieldName) where T : BaseEntity
        {
            validation.KeyValuesOk = true;

            var properties = entity.GetType().GetKeyProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributeAny<FluentRandomKeyValueOnAddAttribute>(true)) { continue; }
                if (!string.IsNullOrWhiteSpace(compositionProperty))
                {
                    if(property.GetCustomAttribute<FluentJsonPropertyAttribute>(true)?.form == Core.Enumerator.EnumForm.HIDDEN)
                    {
                        continue;
                    }
                }

                if (property.GetValue(entity).IsFluentNull())
                {
                    validation.AddInconsistency(new UiFieldRequiredFluentValidationException(property, compositionProperty, compositionFieldName));
                    validation.KeyValuesOk = false;
                }
            }
        }

        internal static void AllKeysShouldBeInformedWhenThereAreMoreThanOne<T>(this IFluentValidation validation, T entity, string compositionProperty, string compositionFieldName, bool isUpdate = false) where T : BaseEntity
        {
            if (!validation.NullParameterOk || !validation.KeyValuesOk)
            {
                return;
            }

            var entityType = typeof(T);
            var properties = entityType.GetKeyProperties();
            if (properties.Count > 1)
            {
                validation.AllKeysMustBeInformed(entity, compositionProperty, compositionFieldName);
            }
            else
            {
                var property = properties.First();
                if (property.GetValue(entity).IsFluentNull())
                {
                    if (!isUpdate)
                    {
                        return;
                    }

                    validation.AddInconsistency(new UiFieldRequiredFluentValidationException(property, compositionProperty, compositionFieldName));
                    validation.KeyValuesOk = false;
                }
                else
                {
                    if (isUpdate)
                    {
                        return;
                    }

                    //Todo - Exigir que não seja informado somente quando o campo for de auto incremento.
                    //AddInconsistency(new DbFieldNotRequiredFluentValidationException(property));
                    //KeyValuesOk = false;
                }
            }
        }

        internal static async Task EntityMustExistInDatabaseAsync<T>(this IFluentValidation validation, T entity, bool includeExcludedLogically = false) where T : BaseEntity
        {
            if (!validation.NullParameterOk)
            {
                return;
            }

            if (!await validation.FluentCast<FluentValidation<T>>().Service.ExistsAsync(entity, validation.KeyValuesOk, includeExcludedLogically))
            {
                var keys = entity.GetKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                validation.AddInconsistency(new EntityNotFoundFluentValidationException(keyValues));
            }
        }

        internal static async Task ThereIsOnlyOneEntityAsync<T>(this IFluentValidation validation, T entity, bool includeExcludedLogically = false) where T : BaseEntity
        {
            if (!validation.NullParameterOk)
            {
                return;
            }

            if (await validation.FluentCast<FluentValidation<T>>().Service.CountAsync(entity, includeExcludedLogically) > 1)
            {
                var keys = entity.GetKeyValues().Select(x => $"-{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                validation.AddInconsistency(new EntityExistsFluentValidationException(keyValues));
            }
        }

        internal static async Task EntityShouldNotExistInDatabaseBasedOnKeysAsync<T>(this IFluentValidation validation, T entity, bool checkId) where T : BaseEntity
        {
            if (!validation.NullParameterOk || !validation.KeyValuesOk)
            {
                return;
            }

            if (await validation.FluentCast<FluentValidation<T>>().Service.ExistsAsync(entity, checkId))
            {
                var keys = entity.GetKeyAndFluentUniqueKeyValues().Select(x => $"{{{x.Property.Name}:{x.Value}}}").ToArray();
                var keyValues = string.Join(", ", keys);
                validation.AddInconsistency(new EntityExistsFluentValidationException(keyValues));
            }
        }
    }
}
