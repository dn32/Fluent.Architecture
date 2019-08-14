// -----------------------------------------------------------------------
// <copyright company="Fluent System">
//     Copyright © Fluent System. All rights reserved.
//     TODOS OS DIREITOS RESERVADOS.
// </copyright>
// -----------------------------------------------------------------------

using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;

namespace Fluent.Architecture.Sample.Test.SupportElements.Model
{
    /// <inheritdoc />
    [DbType(FluentDbType.SQLITE)]
    public class Course : FluentGlobalizedEntity
    {
        [FluentGlobalization]
        public string Title { get; set; }

        [FluentGlobalization]
        public string Description { get; set; }
    }
}
