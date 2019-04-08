using Fluent.Architecture.Core.Attributes;
using Fluent.Architecture.Enumerator;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.EntityFramework.SqlServer
{
    [DbType(FluentDbType.ORACLE)]
    public class EntidadeTeste : FluentEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
