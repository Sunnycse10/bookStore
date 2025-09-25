namespace Book_App.DTOs
{
    public class BookDTO
    {
        public int Id {  get; set; }
        public string? Title {  get; set; }
        public string? ISBN_10;
        public decimal Price;
        public ICollection<CategoryDTO> Categories { get; set; }=new List<CategoryDTO>();
        public ICollection<AuthorInfoDTO> Authors { get; set; }=new List<AuthorInfoDTO>();
    }
}
