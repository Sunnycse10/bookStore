using Book_App.DTOs;
using Book_App.Models;
using Book_App.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Book_App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class booksController:ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        public booksController(IBookService bookService, IAuthorService authorService)
        {
            _bookService = bookService;
            _authorService = authorService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks() =>
            Ok(await _bookService.GetAll().Select(b => new BookDTO { Id = b.Id, Title = b.Title, ISBN_10=b.ISBN_10,
                Authors = b.Authors.Select(a=>new AuthorInfoDTO { Id=a.Id, Name=a.Name}).ToList(),
                Categories=b.Categories.Select(c=>new CategoryDTO {Id=c.Id, Title=c.Title }).ToList(),
                Price=b.Price}));

        

        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);
            if(book == null) { return NotFound(); }
            else return Ok(new BookDTO { Id= book.Id, Title=book.Title,
                Authors = book.Authors.Select(b=> new AuthorInfoDTO { Id = b.Id, Name = b.Name }).ToList(),
                Categories = book.Categories.Select(c => new CategoryDTO { Id=c.Id, Title = c.Title }).ToList(),
                Price =book.Price});
        }
        [HttpPost]
        public ActionResult<BookDTO> CreateBook(CreateBookDTO bookDto)
        {
            
            var created = _bookService.Add(bookDto);
            if (created == null) { return NotFound(); }

            return CreatedAtAction(nameof(GetBookById), new { id = created.Id }, new BookDTO
            {
                Id = created.Id,
                ISBN_10 = created.ISBN_10,
                Title = created.Title,
                Price = created.Price,
                Authors = created.Authors.Select(a=> new AuthorInfoDTO
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList(),
                Categories= created.Categories.Select(a=> new CategoryDTO { Id=a.Id, Title=a.Title}).ToList()
            });
        }

        [HttpGet("category/{id}")]
        public ActionResult<CategoryWithBooksDTO> GetCategoryById(int id)
        {
            var category = _bookService.GetCategoryById(id);
            if (category == null) { return NotFound(); }
            return Ok(new CategoryWithBooksDTO { Id = category.Id, Title = category.Title ,
                Books = category.Books.Select(b=> new BookInfoDTO
                { Id=b.Id,Title=b.Title, ISBN_10=b.ISBN_10, Price=b.Price}).ToList()} );

        }

        [HttpPost("category")]
        public ActionResult<CategoryDTO> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            var category = new Category { Title = createCategoryDTO.Title };
            var created = _bookService.AddCategory(category);
            if (created == null) { return NotFound(); }
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, new CategoryDTO { Id = created.Id, Title = created.Title });
            
        }
        
        [HttpPut("{id}")]
        public ActionResult<BookDTO> UpdateBook(int id, CreateBookDTO book)
        {            
            var updated = _bookService.Update(id,book);
            return new BookDTO
            {
                Id = updated.Id,
                ISBN_10 = updated.ISBN_10,
                Title = updated.Title,
                Price = updated.Price,
                Authors = updated.Authors.Select(a => new AuthorInfoDTO
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList(),
                Categories = updated.Categories.Select(a=> new CategoryDTO { Id=a.Id,Title=a.Title}).ToList(),
            };
            
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id) 
        {
            if(_bookService.GetBookById(id) == null) {  return NotFound(); }
            _bookService.DeleteById(id);
            return NoContent();

        }



    }
}
