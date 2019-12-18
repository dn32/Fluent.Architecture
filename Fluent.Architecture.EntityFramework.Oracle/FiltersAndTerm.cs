using Fluent.Architecture.Core.Enumerator;
using System.ComponentModel;

namespace Fluent.Architecture.EntityFramework.Oracle
{
    public class FiltersAndTerm
    {
        [Description("Query Filters")]
        public Filter[] Filters { get; set; }

        [Description("The properties whose value will be compared")]
        public string[] Properties { get; set; }

        [Description("The term to compare with the property value")]
        public string Term { get; set; }

        [Description("The tolerance of comparing term and property value")]
        public int Tolerance { get; set; }
    }
}
