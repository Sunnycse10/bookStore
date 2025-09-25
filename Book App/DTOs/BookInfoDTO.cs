namespace Book_App.DTOs
{
    public class BookInfoDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? ISBN_10 { get; set; }
    }
}
