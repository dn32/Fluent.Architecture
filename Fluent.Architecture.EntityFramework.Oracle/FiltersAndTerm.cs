using Fluent.Architecture.Core.Enumerator;

namespace Fluent.Architecture.EntityFramework.Oracle
{
    public class FiltersAndTerm
    {
        public Filter[] Filters { get; set; }
        public string Property { get; set; }
        public string Term { get; set; }
        public int Tolerance { get; set; }
    }
}
