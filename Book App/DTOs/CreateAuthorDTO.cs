using System.ComponentModel.DataAnnotations;

namespace Book_App.DTOs
{
    public class CreateAuthorDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
