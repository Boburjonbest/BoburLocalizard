using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Localizard.Models
{
   
    public class Project
    {
        public int Id { get; set; }
      
        public string UserId { get; set; } = string.Empty;
        public string? Name { get; set; } = null!;
        public string? DefaultLAnguage { get; set; } = null!;
        [NotMapped]
        public List<string> AvailableLanguage { get; set; } = new List<string>();
        [JsonPropertyName("CreatedAt")]
      
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [JsonPropertyName("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;



      
    }







}
