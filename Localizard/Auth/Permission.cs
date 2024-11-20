using System.ComponentModel.DataAnnotations;

namespace Localizard.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
    }
}
