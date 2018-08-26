using Fluent.Architecture.Attributes;
using Fluent.Architecture.Exception.ValidationException;
using Fluent.Architecture.Test.SupportElements.Model;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Test.SupportElements
{
    internal class UserValidation : FluentValidation<User>
    {
        public override void Add(User entity)
        {
            base.Add(entity);
            UserMustHaveFullName(entity);

            RunTheContextValidation();
        }

        public override void Update(User entity)
        {
            base.Update(entity);
            UserMustHaveFullName(entity);

            RunTheContextValidation();
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

        [Propagate]
        public void FindById(int id)
        {
            if (id == 0)
            {
                AddInconsistency(new FluentParameterValidationException(nameof(id), $"The {nameof(id)} parameter can not be 0 for this operation."));
            }

            RunTheContextValidation();
        }
    }
}