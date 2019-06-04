using Fluent.Architecture.Validation;

namespace Max.Infraestrutura.ClassesBase
{
    public abstract class MaxValidacao<T> : FluentValidation<T> where T : MaxEntidade
    {
    }
}
