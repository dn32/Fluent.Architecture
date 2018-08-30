using Fluent.Architecture.Model;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Services;

namespace Fluent.Architecture.Sample.Test.SupportElements.Services
{
    public class BaseServiceTest
    {
        public class LocalTestService : FluentService<TestEntity>
        {
            public void SetUserSessionForTest(UserSessionRequest sessionRequest)
            {
                this.SetUserSession(sessionRequest);
            }
        }
    }
}