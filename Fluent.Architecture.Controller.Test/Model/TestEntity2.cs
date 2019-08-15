// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Attributes;
using Fluent.Architecture.Entities;

namespace Fluent.Architecture.Controller.Test.Model
{
    [NotDbEntity]
    public class TestEntity2 : FluentEntity
    {
        public int Id { get; set; }
    }
}