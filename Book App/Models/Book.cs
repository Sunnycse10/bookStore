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
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (value <= 0) throw new ArgumentException("Price cannot be negative");
                _price = value;
            }
        }
        public ICollection<Author> Authors { get; set; }=new List<Author>();
        public ICollection<Category> Categories { get; set; }=new List<Category>();
    }
}
