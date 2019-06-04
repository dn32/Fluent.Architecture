using Fluent.Architecture.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Max.Infraestrutura.ClassesBase
{
    public abstract class MaxController<T> : FluentController<T> where T : MaxEntidade
    {
       
    }
}
