// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Fluent.Architecture.Test
{
    public static class FluentAssert
    {
        public static void Equal(object obj1, object obj2)
        {
            if (!obj1.CompareObjects(obj2))
            {
                throw new ValidationException("The objects are different");
            }
        }

        public static void IsNotNullOrEmpty(object entity)
        {
            if (entity.IsFluentNull())
            {
                throw new ValidationException("The text is empty, null or space");
            }
        }
    }
}