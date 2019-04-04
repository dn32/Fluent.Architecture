// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Attributes;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Sample.Test.SupportElements.Model
{
    [NotDbEntity]
    public class TestEntity2 : FluentEntity
    {
        public int Id { get; set; }
    }
}