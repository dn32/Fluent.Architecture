using Fluent.Architecture.Extensions;

namespace Fluent.Architecture.Test
{
    using System.ComponentModel.DataAnnotations;

    public static class FluentAssert
    {
        public static void Equal(object obj1, object obj2)
        {
            if (!obj1.CompareObjects(obj2))
            {
                throw new ValidationException("The objects are different");
            }
        }

        public static void IsNotNullOrEmpty(object obj)
        {
            if (obj.IsFluentNull())
            {
                throw new ValidationException("The text is empty, null or space");
            }
        }
    }
}