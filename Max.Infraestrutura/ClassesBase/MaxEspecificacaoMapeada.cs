using Fluent.Architecture.Specifications;

namespace Max.Infraestrutura.ClassesBase
{
    public abstract class MaxEspecificacaoMapeada<TE, TO> : FluentSelectSpecification<TE, TO> where TE : MaxEntidade
    {
    }
}
