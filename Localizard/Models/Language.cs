using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Localizard.Models
{
    public class Language
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string[]? PluralForms { get; set; } = { };
        public string? LanguageCode { get; set; }

      
    }
}
