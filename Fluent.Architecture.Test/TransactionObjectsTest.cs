using System.Linq;
using Fluent.Architecture.Repository;
using Fluent.Architecture.Test.Mock;

namespace Fluent.Architecture.Test
{
    public class TransactionObjectsTest : TransactionObjects
    {
        public TransactionObjectsTest(string dataBaseConnectionString) : base(dataBaseConnectionString)
        {
        }

        protected override IQueryable<TX> GetObjectQueryInternal<TX>() 
        {
            var list = FluentMockUtil.GetObjectQueryForMock<TX>();

            if (list == null)
            {
                return base.GetObjectQueryInternal<TX>();
            }

            return list;
        }
    }
}
