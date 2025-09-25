using System.ComponentModel.DataAnnotations;

namespace Book_App.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public ICollection<Book> Books { get; set; }= new List<Book>();
    }
}
