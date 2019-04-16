using Fluent.Architecture.Attributes;
using Fluent.Architecture.Entities;
using Fluent.Architecture.EntityFramework;
using Fluent.Architecture.EntityFramework.MySQL;
using Fluent.Architecture.EntityFramework.SqlServer;
using System.ComponentModel.DataAnnotations;

namespace Fluent.Architecture.Sample
{
    [DbType(FluentDbType.MYSQL)]
    public class Client : FluentIdEntity
    {
        [FluentUniqueKey]
        public string Email { get; set; }

        public string Name { get; set; }
    }

    public class ClientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
