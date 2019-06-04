using Fluent.Architecture.Specifications;

namespace Max.Infraestrutura.ClassesBase
{
    public abstract class MaxEspecificacao<T> : FluentSpecification<T> where T : MaxEntidade
    {
    }
}
