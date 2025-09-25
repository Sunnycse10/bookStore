namespace Book_App.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<BookInfoDTO> Books { get; set; } = new List<BookInfoDTO>();
    }
}
