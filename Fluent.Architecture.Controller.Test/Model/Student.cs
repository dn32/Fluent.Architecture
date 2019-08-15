// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;

namespace Fluent.Architecture.Controller.Test.Model
{
    [DbType(FluentDbType.SQLITE)]
    public class Student : FluentEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [FluentUniqueKey]
        public string Document { get; set; }

        public string Email { get; set; }
    }
}
