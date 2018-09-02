using System.ComponentModel.DataAnnotations;

namespace Fluent.Architecture.Model
{
    /// <inheritdoc />
    public abstract class FluentGlobalizedEntity : FluentEntity
    {
        [MaxLength(5)]
        public string Language { get; set; }

        public bool IsDefaultLanguage { get; set; }
    }
}