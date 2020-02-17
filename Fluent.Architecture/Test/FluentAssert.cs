// -----------------------------------------------------------------------
// <copyright company="Fluente System">
//     Copyright © Fluente System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluente.Arquitetura.Extensoes;
using System.ComponentModel.DataAnnotations;

namespace Fluente.Arquitetura.Test
{
    public static class FluenteAssert
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
            if (entity.IsFluenteNull())
            {
                throw new ValidationException("The text is empty, null or space");
            }
        }
    }
}