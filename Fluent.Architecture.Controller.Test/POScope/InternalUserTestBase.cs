using Fluent.Architecture.Test;

namespace Fluent.Architecture.Controller.Test.POScope
{
    internal class InternalUserTestBase : FluentTest<User>
    {
        public InternalUserTestBase()
        {
            Architecture.Test.Setup.Initialize(null);
        }

        protected void SetCategory(int category)
        {
            Category = category;
        }

        private int Category { get; set; }

        public override User GetNew()
        {
            var rand = TestUtil.NextRandom();
            if (Category == 0) { Category = rand; }

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
                Category = Category
            };
        }
    }
}
