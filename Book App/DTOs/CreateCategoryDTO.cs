using System.ComponentModel.DataAnnotations;

namespace Book_App.DTOs
{
    public class CreateCategoryDTO
    {
        [Required]
        public string Title { get; set; } 
    }
}
