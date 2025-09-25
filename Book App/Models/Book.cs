using System.ComponentModel.DataAnnotations;

namespace Book_App.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; } 
        public string? Image { get; set; }
        public string? ISBN_10 { get; set; }
        public decimal Price { get; set; }
        public ICollection<Author> Authors { get; set; }=new List<Author>();
        public ICollection<Category> Categories { get; set; }=new List<Category>();
    }
}
