using Fluent.Architecture.Model;
using Fluent.Architecture.Service;
using Fluent.Architecture.Test.SupportElements.Model;

namespace Fluent.Architecture.Test.SupportElements.Services
{
    internal class BaseServiceTest
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