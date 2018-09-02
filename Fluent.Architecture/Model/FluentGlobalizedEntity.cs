using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fluent.Architecture.Model
{
    /// <inheritdoc />
    public abstract class FluentGlobalizedEntity : FluentEntity
    {
        [MaxLength(5)]
        public string Language { get; set; }

        [NotMapped] public bool IsDefaultLanguage { get; set; }

        protected FluentGlobalizedEntity()
        {
            IsDefaultLanguage = true;
        }
    }
}