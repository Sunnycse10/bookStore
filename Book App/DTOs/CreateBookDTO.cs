

using System.ComponentModel.DataAnnotations;

namespace Book_App.DTOs
{
    public class CreateBookDTO
    {
        [Required]
        public string Title { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string ISBN_10 {  get; set; }
        public ICollection<int> CategoryIds { get; set; } = new List<int>();
        public ICollection<int> AuthorIds { get; set; } = new List<int>();

    }
}
