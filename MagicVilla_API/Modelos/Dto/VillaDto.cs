using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos.Dtos
{
    public class VillaDto
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
