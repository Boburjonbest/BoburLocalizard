using System.Text.Json.Serialization;

namespace Localizard.Models
{
    public class Tag
    {
        public int Id { get; set; }
    
        
        public string Text { get; set; }
    }

    public class Tags
    {
        public int SomeObjectId {  get; set; }
        public string Name { get; set; }
        public List<Tags> Tag { get; set; }
        
    }
}
