using Fluent.Architecture.Controllers;
using Fluent.Architecture.Test;
using Fluent.Architecture.Test.Mock;
using Fluent.Architecture.Test.Mock.ControllerMock;
using Newtonsoft.Json;
using System;

namespace Fluent.Architecture.Controller.Test.POScope
{
    internal class InternalUserTestBase
    {
        public InternalUserTestBase()
        {
            Architecture.Test.Setup.Initialize(null);
        }

        protected UserController GetNewController()
        {
            return MockUtil.GetMockController<UserController>();
        }

        internal bool RemoveUser(User user)
        {
            return Execute<bool>((UserController controller) => controller.Remove(user));
        }

        internal User AddUser(User user)
        {
            return Execute<User>((UserController controller) => controller.Add(user));
        }

        internal User AddNewUser()
        {
            return Execute<User>((UserController controller) => controller.Add(GetNewUser()));
        }

        internal User[] AddRangeUsers(User[] users)
        {
            return Execute<User[]>((UserController controller) => controller.AddRange(users));
        }

        internal User FindUser(User user)
        {
            return Execute<User>((UserController controller) => controller.Find(user));
        }

        internal void RemoveRangeUser(User[] users)
        {
            Execute<User[]>((UserController controller) => controller.RemoveRange(users));
        }

        internal bool UpdateUser(User user)
        {
            return Execute<bool>((UserController controller) => controller.Update(user));
        }

        public static User GetNewUser()
        {
            var rand = TestUtil.NextRandom();
            return new User
            {
                PersonType = EnumPersonType.User,
                Id = rand,
                UserName = $"maria {rand}",
                Name = $"maria {rand}",
                Email = $"test{rand}@mail.com",
                Password = $"test{rand}@mail.com",
                Tel = $"test{rand}@mail.com",
                ZipCode = rand,
                Category = rand
            };
        }

        public TR Execute<TR>(Func<UserController, DefaultResult> actionMethod)
        {
            var currentController = GetNewController();
            currentController.OnActionExecuting(MockActionExecutingContextFactory.Create(currentController));
            var ret = actionMethod(currentController);
            currentController.OnActionExecuted(MockActionExecutedContextFactory.Create(currentController));
            return JsonConvert.DeserializeObject<TR>(JsonConvert.SerializeObject(ret.Data));
        }
    }
}
