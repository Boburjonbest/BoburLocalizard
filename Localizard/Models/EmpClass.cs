using System.ComponentModel.DataAnnotations;

namespace Localizard.Models
{
    public class EmpClass
    {
        [Key]

        public string? Name { get; set; } = null!;

        public string? DefaultLanguage { get; set; } = null!;

        public string? AvailableLanguage { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }



}
