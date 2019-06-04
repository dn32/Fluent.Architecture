using Fluent.Architecture.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;

namespace Fluent.Architecture.Sample
{
    [DbType(FluentDbType.MYSQL)]
    public class Client : FluentIdEntity
    {
        [FluentUniqueKey]
        public string Email { get; set; }

        public string Name { get; set; }
    }
}
