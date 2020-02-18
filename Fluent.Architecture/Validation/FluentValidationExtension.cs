using Fluente.Arquitetura.Extensoes;
using Fluente.Arquitetura.Factory;
using Fluente.Arquitetura.Nucleo.Atributos;
using Fluente.Arquitetura.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using dn32.infra.dados;
using dn32.infra.extensoes;

namespace Fluente.Arquitetura.Validation
{
    internal static class FluenteValidationExtension
    {
        internal static async Task<List<TransactionalService>> ExecuteEntityAndCompositions<T>(this FluenteValidation<T> validation, object entity, MethodInfo method) where T : EntidadeBase
        {
            if (validation is null) { throw new ArgumentNullException("validation"); }
            if (method is null) { throw new ArgumentNullException("method"); }

            var tasks = new List<Task>();
            var t1 = method.MakeGenericMethod(typeof(T)).Invoke(null, new object[] { validation, entity, null, null }).FluenteCast<Task>();
            if (t1 != null) { tasks.Add(t1); }

            List<TransactionalService> anotherServices = new List<TransactionalService>();

            if (entity != null)
            {
                var properties = entity.GetType().GetProperties().ToList().Where(x => x.IsDefined(typeof(FluenteCompositionAttribute))).ToList();
                foreach (var property in properties)
                {
                    var entityCompositionValue = property.GetValue(entity);
                    var entityType = property.PropertyType.GetListTypeNonNull();
                    if (!entityType.IsFluenteEntity()) { continue; }

                    var service = ServiceFactory.Create(entityType, validation.SessionRequest.LocalHttpContext, "For multiple validation");
                    anotherServices.Add(service);

                    if (property.PropertyType.IsList())
                    {
                        if (!(entityCompositionValue is IEnumerable collection))
                        {
                            continue;
                        }

                        int i = 0;
                        foreach (var item in collection)
                        {
                            var compositionPropertyName = $"{property.GetJsonPropertyName()}[{i}]";
                            var compositionFieldName = $"{property.GetUiPropertyName()}[{i}]";

                            var t2 = method.MakeGenericMethod(entityType).Invoke(null, new object[] { service.Validation, item, compositionPropertyName, compositionFieldName }).FluenteCast<Task>();
                            if (t2 != null) { tasks.Add(t2); }
                            i++;
                        }
                    }
                    else
                    {
                        var compositionPropertyName = property.GetJsonPropertyName();
                        var compositionFieldName = property.GetUiPropertyName();

                        var t2 = method.MakeGenericMethod(entityType).Invoke(null, new object[] { service.Validation, entityCompositionValue, compositionPropertyName, compositionFieldName }).FluenteCast<Task>();
                        if (t2 != null) { tasks.Add(t2); }
                    }
                }
            }

            await Task.WhenAll(tasks.ToArray());
            return anotherServices;
        }
    }
}
