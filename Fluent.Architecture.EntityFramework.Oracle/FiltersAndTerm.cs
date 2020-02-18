using System.ComponentModel;
using dn32.infra.dados;

namespace dn32.infra.EntityFramework.Oracle
{
    public class FiltersAndTerm
    {
        [Description("Query Filters")]
        public Filtro[] Filters { get; set; }

        [Description("The properties whose valor will be compared")]
        public string[] Properties { get; set; }

        [Description("The term to compare with the property valor")]
        public string Term { get; set; }

        [Description("The tolerance of comparing term and property valor")]
        public int Tolerance { get; set; }
    }
}
