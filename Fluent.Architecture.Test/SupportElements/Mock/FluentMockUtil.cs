using System.Collections;
using System.Linq;
using System.Text;
using Fluent.Architecture.Extensions;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Test.SupportElements.Mock
{
    public class FluentMockUtil
    {
        public static IQueryable<TX> GetObjectQueryForMock<TX>() where TX : BaseEntity
        {
            var type = typeof(TX);
            if (Setup.LocalContext.TryGetValue(type, out var value))
            {
                return value as IQueryable<TX>;
            }

            return null;
        }

        public static void SetObjectQueryForMock(params IList[] listOfList)
        {
            foreach (var item in listOfList)
            {
                var type = item.GetType().GetGenericArguments().First();
                if (Setup.LocalContext.ContainsKey(type))
                {
                   Setup.LocalContext.Remove(type);
                }

                Setup.LocalContext.Add(type, item.AsQueryable());
            }
        }

        public static string CreateKeyForMock(string fullName, string methodName, object[] parameters)
        {
            var content = new StringBuilder();

            parameters.ToList().ForEach(x => content.Append(x.GetAllDataOfObject()));

            return fullName + "." + methodName + "((" + content + "))";
        }

        public static string CreateKeyForMock(FluentInvocation invocation)
        {
            return CreateKeyForMock(invocation.TargetType.FullName, invocation.Method.GetFriendlyName(), invocation.Arguments);
        }
    }
}