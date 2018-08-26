using Fluent.Architecture.Extensions;

namespace Fluent.Architecture.Test
{
    public static class FluentAssert
    {
        public static void Equal(object obj1, object obj2)
        {
            if (!obj1.CompareObjects(obj2))
            {
                throw new System.Exception("The objects are different");
            }
        }
    }
}