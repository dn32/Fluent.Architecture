using System;
using System.Threading.Tasks;

namespace Fluent.Architecture.Core.Extensions
{
    //Todo - 001 Testar
    public static class ObjectExtension
    {
        public static bool TypeIsTask(this Type type) => type == typeof(Task);

        public static bool TypeIsTaskT(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);

        public static object FluentResultOrValue(this object data)
        {
            if (data == null) { return default; }

            var type = data.GetType();

            if (type.TypeIsTask())
            {
                InvokeTask(data);
                return null;
            }
            else if (type.TypeIsTaskT())
            {
                return InvokeTaskT(data);
            }

            return data;
        }

        private static void InvokeTask(object data) => data.GetType().GetMethod(nameof(Task.Wait))?.Invoke(data, null);

        private static object InvokeTaskT(object data) => data.GetType().GetProperty("Result")?.GetValue(data);
    }
}
