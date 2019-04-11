using Fluent.Architecture.EntityFramework.SqlServer;

namespace Fluent.Architecture.Sample
{
    public class EntidadeTeste : FluentSQLEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
