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

        internal User AddNewUser(int category = 0)
        {
            return Execute<User>((UserController controller) => controller.Add(GetNewUser(category)));
        }

        internal bool AddRangeUsers(User[] users)
        {
            return Execute<bool>((UserController controller) => controller.AddRange(users));
        }

        internal User FindUser(User user)
        {
            return Execute<User>((UserController controller) => controller.Find(user));
        }

        internal bool RemoveRangeUser(User[] users)
        {
            return Execute<bool>((UserController controller) => controller.RemoveRange(users));
        }

        internal bool UpdateUser(User user)
        {
            return Execute<bool>((UserController controller) => controller.Update(user));
        }

        public static User GetNewUser(int category = 0)
        {
            var rand = TestUtil.NextRandom();
            if (category == 0) { category = rand; }

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
                Category = category
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
