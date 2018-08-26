#if NET461
using Fluent.Architecture.Model;
using Fluent.Architecture.Service;

namespace Fluent.Architecture.Test.InternalTests
{
    public partial class BaseServiceTest
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
#endif