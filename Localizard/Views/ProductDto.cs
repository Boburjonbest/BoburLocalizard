using Localizard.Models;

namespace Localizard.Views
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string  Namekeys { get; set; }
        public int[] Tags { get; set; }
        public List<ObyektTranslation> Translations { get; set; } = new();

        public ProductDto()
        {
            Translations = new List<ObyektTranslation>();
        }
    }
}
