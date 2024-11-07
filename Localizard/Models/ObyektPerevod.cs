using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;



namespace Localizard.Models
{
    public class ObyektPerevod
    {
        public int Id { get; set; }
        public string Namekeys { get; set; }
      
        public string Description { get; set; }
        public int[] Tags { get; set; }
      
        public int? ParentId { get; set; }
        public List<ObyektTranslation> Translations { get; set; } = new();

        public ObyektPerevod()
        {
            Translations = new List<ObyektTranslation>();
        }
    }
}
