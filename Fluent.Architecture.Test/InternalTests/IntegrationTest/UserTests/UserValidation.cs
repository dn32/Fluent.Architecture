using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Test.InternalTests.IntegrationTest.UserTests
{
    public class UserValidation : FluentValidation<User>
    {
        public override void Add(User entity)
        {
            base.Add(entity);
            UserMustHaveFullName(entity);
        }

        public override void Update(User entity)
        {
            base.Update(entity);
            UserMustHaveFullName(entity);
        }

        private void UserMustHaveFullName(User user)
        {
            if (!NullParameterOk)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(user.Name) || !user.Name.Trim().Contains(" "))
            {
                AddInconsistency(new FluentPropertyValidationException(nameof(user.Name), "User must have full name"));
            }
        }
    }
}