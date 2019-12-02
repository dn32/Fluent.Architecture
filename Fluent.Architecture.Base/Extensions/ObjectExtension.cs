using Fluent.Architecture.Extensions;
using System.Threading.Tasks;

namespace Fluent.Architecture.Core.Extensions
{
    public static class ObjectExtension
    {
        public static object FluentResultOrValue(this object data)
        {
            if (data != null)
            {
                var type = data.GetType();
                if (type == typeof(Task))
                {
                    data.GetType().GetMethod(nameof(Task.Wait)).Invoke(data, null);
                    return null;
                }

                if (type.IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(Task<>))
                {
                    return data.GetType().GetProperty("Result").GetValue(data);
                }

                return data;
            }

            return null;
        }

        public static T FluentResultOrValue<T>(this object data)
        {
            if (data != null)
            {
                var type = data.GetType();
                if (type == typeof(Task))
                {
                    data.GetType().GetMethod(nameof(Task.Wait)).Invoke(data, null);
                    return default;
                }

                if (type.IsGenericType && data.GetType().GetGenericTypeDefinition() == typeof(Task<>))
                {
                    return data.GetType().GetProperty("Result").GetValue(data).FluentCast<T>();
                }

                return data.FluentCast<T>();
            }

            return default;
        }
    }
}
