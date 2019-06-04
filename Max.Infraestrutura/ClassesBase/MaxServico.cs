using Fluent.Architecture.Services;

namespace Max.Infraestrutura.ClassesBase
{
    public abstract class MaxServico<T> : FluentService<T> where T : MaxEntidade
    {
    }
}
