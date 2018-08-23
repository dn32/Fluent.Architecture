using System.Reflection;

namespace Fluent.Architecture.Factory.Interface
{
    internal interface IFluentDynamicProxy
    {
        MethodInfo TargetMethod { get; set; }
    }
}