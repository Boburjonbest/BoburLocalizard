using System.Text.Json.Serialization;

namespace Localizard.Models
{
    public class ObyektTranslation
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Language { get; set; }
        public string Text { get; set; }

        [JsonIgnore]
        public int ObyektPerevodId { get; set; }

    }
}
