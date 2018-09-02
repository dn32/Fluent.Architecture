using System.ComponentModel.DataAnnotations.Schema;
using Fluent.Architecture.Model;

namespace Fluent.Architecture.Sample.Test.SupportElements.Model
{
    [Table("fluent_language")]
    public class Language : FluentEntity
    {
        public int Id { get; set; }

        public string Acronym { get; set; }

        public Language()
        {
        }

        public Language(int id, string acronym)
        {
            Id = id;
            Acronym = acronym;
        }

        public static Language DefaultLanguage => new Language(0, "en-US");

        public static Language Get(string acronym)
        {
           return new Language(-1, acronym);
        }
    }
}