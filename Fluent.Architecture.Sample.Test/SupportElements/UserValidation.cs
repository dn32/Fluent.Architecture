using Fluent.Architecture.Attributes;
using Fluent.Architecture.Exceptions.ValidationException;
using Fluent.Architecture.Sample.Test.SupportElements.Model;
using Fluent.Architecture.Validation;

namespace Fluent.Architecture.Sample.Test.SupportElements
{
public class UserValidation : FluentValidation<User>
{
    public override void Add(User entity)
    {
        UserMustHaveFullName(entity);
        base.Add(entity);
    }

    public override void Update(User entity)
    {
        UserMustHaveFullName(entity);
        base.Update(entity);
    }

    private void UserMustHaveFullName(User user)
    {
        if (string.IsNullOrWhiteSpace(user?.Name) || !user.Name.Trim().Contains(" "))
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

        // For ambiguity test
        [Propagate]
        public void Test2(string data) { }

        // For ambiguity test
        [Propagate]
        public void Test2(int data) { }
    }
}