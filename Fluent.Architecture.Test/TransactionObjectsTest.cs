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
            return FluentMockUtil.GetObjectQueryForMock<TX>();
        }
    }
}
