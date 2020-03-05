using dn32.infra.Factory;
using dn32.infra.nucleo.atributos;
using dn32.infra.servicos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using dn32.infra.dados;
using dn32.infra.extensoes;

namespace dn32.infra.Validation
{
    internal static class DnValidationExtension
    {
        internal static async Task<List<DnServicoTransacionalBase>> ExecuteEntityAndCompositions<T>(this DnValidation<T> validation, object entity, MethodInfo method) where T : EntidadeBase
        {
            if (validation is null) { throw new ArgumentNullException("validation"); }
            if (method is null) { throw new ArgumentNullException("method"); }

            var tasks = new List<Task>();
            var t1 = method.MakeGenericMethod(typeof(T)).Invoke(null, new object[] { validation, entity, null, null }).DnCast<Task>();
            if (t1 != null) { tasks.Add(t1); }

            List<DnServicoTransacionalBase> anotherServices = new List<DnServicoTransacionalBase>();

            if (entity != null)
            {
                var properties = entity.GetType().GetProperties().ToList().Where(x => x.IsDefined(typeof(DnComposicaoAtributo))).ToList();
                foreach (var property in properties)
                {
                    var entityCompositionValue = property.GetValue(entity);
                    var entityType = property.PropertyType.GetListTypeNonNull();
                    if (!entityType.IsDnEntity()) { continue; }

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

                            var t2 = method.MakeGenericMethod(entityType).Invoke(null, new object[] { service.Validacao, item, compositionPropertyName, compositionFieldName }).DnCast<Task>();
                            if (t2 != null) { tasks.Add(t2); }
                            i++;
                        }
                    }
                    else
                    {
                        var compositionPropertyName = property.GetJsonPropertyName();
                        var compositionFieldName = property.GetUiPropertyName();

                        var t2 = method.MakeGenericMethod(entityType).Invoke(null, new object[] { service.Validacao, entityCompositionValue, compositionPropertyName, compositionFieldName }).DnCast<Task>();
                        if (t2 != null) { tasks.Add(t2); }
                    }
                }
            }

            await Task.WhenAll(tasks.ToArray());
            return anotherServices;
        }
    }
}
