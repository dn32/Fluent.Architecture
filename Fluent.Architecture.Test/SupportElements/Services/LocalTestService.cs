using Fluent.Architecture.Model;
using Fluent.Architecture.Services;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Services
{
    public class BaseServiceTest
    {
        public class LocalTestService : FluentService<TestEntity>
        {
            public void SetUserSessionForTest(UserSessionRequest sessionRequest)
            {
                SetUserSession(sessionRequest);
            }
        }
    }
}