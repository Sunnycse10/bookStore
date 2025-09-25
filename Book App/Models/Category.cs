using System.ComponentModel.DataAnnotations;

namespace Book_App.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
