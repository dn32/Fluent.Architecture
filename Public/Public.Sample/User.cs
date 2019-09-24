using Fluent.Architecture.Entities;
using System.ComponentModel.DataAnnotations;

namespace Public.SimpleApiSample
{
    /* Example model */
    public class User : FluentEntity
    {
        [Key]
        public long Code { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}