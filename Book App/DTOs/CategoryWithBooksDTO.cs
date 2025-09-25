namespace Book_App.DTOs
{
    public class CategoryWithBooksDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public ICollection<BookInfoDTO> Books { get; set; }=new List<BookInfoDTO>(); 
    }
}
