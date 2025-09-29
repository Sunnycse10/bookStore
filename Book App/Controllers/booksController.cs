using AutoMapper;
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
        private readonly IMapper _mapper;
        public booksController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks()
        {
            var books = await _bookService.GetAll();
            var bookDTOs= _mapper.Map<List<BookDTO>>(books); 
            return Ok(bookDTOs
            //    books.Select(b => new BookDTO
            //{
            //    Id = b.Id,
            //    Title = b.Title,
            //    ISBN_10 = b.ISBN_10,
            //    Authors = b.Authors.Select(a => new AuthorInfoDTO { Id = a.Id, Name = a.Name }).ToList(),
            //    Categories = b.Categories.Select(c => new CategoryDTO { Id = c.Id, Title = c.Title }).ToList(),
            //    Price = b.Price
            //})
                );
        }

              

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            var book = await _bookService.GetBookById(id);
            if(book == null) { return NotFound(); }
            var dto = _mapper.Map<BookDTO>(book);
            return Ok(dto);
            //else return Ok(new BookDTO { Id= book.Id, Title=book.Title,
            //    Authors = book.Authors.Select(b=> new AuthorInfoDTO { Id = b.Id, Name = b.Name }).ToList(),
            //    Categories = book.Categories.Select(c => new CategoryDTO { Id=c.Id, Title = c.Title }).ToList(),
            //    Price =book.Price});
        }
        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook(CreateBookDTO bookDto)
        {
            
            var created = await _bookService.Add(bookDto);
            if (created == null) { return NotFound(); }
            var bookDTO= _mapper.Map<BookDTO>(created);

            return CreatedAtAction(nameof(GetBookById), new { id = created.Id }, bookDTO
            //    new BookDTO
            //{
            //    Id = created.Id,
            //    ISBN_10 = created.ISBN_10,
            //    Title = created.Title,
            //    Price = created.Price,
            //    Authors = created.Authors.Select(a=> new AuthorInfoDTO
            //    {
            //        Id = a.Id,
            //        Name = a.Name
            //    }).ToList(),
            //    Categories= created.Categories.Select(a=> new CategoryDTO { Id=a.Id, Title=a.Title}).ToList()
            //}
                );
        }

        [HttpGet("categories/{id}")]
        public async Task<ActionResult<CategoryWithBooksDTO>> GetCategoryById(int id)
        {
            var category = await _bookService.GetCategoryById(id);
            if (category == null) { return NotFound(); }
            var categoryDTO= _mapper.Map<CategoryWithBooksDTO>(category);
            return Ok( categoryDTO
                //new CategoryWithBooksDTO { Id = category.Id, Title = category.Title ,
                //Books = category.Books.Select(b=> new BookInfoDTO
                //{ Id=b.Id,Title=b.Title, ISBN_10=b.ISBN_10, Price=b.Price}).ToList()}
                );

        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<CategoryWithBooksDTO>>> GetAllCategeories()
        {
            var categories = await _bookService.GetAllCategories();
            var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);
            return Ok(categoryDTOs
            //    categories.Select(c => new CategoryWithBooksDTO
            //{
            //    Id = c.Id,
            //    Title = c.Title,
            //    Books = c.Books.Select(b => new BookInfoDTO
            //    { Id = b.Id, Title = b.Title, ISBN_10 = b.ISBN_10, Price = b.Price }).ToList()
            //})
                );
        }


        [HttpPost("categories")]
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            var category = new Category { Title = createCategoryDTO.Title };
            var created = await _bookService.AddCategory(category);
            if (created == null) { return NotFound(); }
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, new CategoryDTO { Id = created.Id, Title = created.Title });
            
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDTO>> UpdateBook(int id, CreateBookDTO book)
        {            
            var updated = await _bookService.Update(id,book);
            var updatedBookDTO = _mapper.Map<BookDTO>(updated);
            return updatedBookDTO;
            //    new BookDTO
            //{
            //    Id = updated.Id,
            //    ISBN_10 = updated.ISBN_10,
            //    Title = updated.Title,
            //    Price = updated.Price,
            //    Authors = updated.Authors.Select(a => new AuthorInfoDTO
            //    {
            //        Id = a.Id,
            //        Name = a.Name
            //    }).ToList(),
            //    Categories = updated.Categories.Select(a=> new CategoryDTO { Id=a.Id,Title=a.Title}).ToList(),
            //};
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id) => await _bookService.DeleteById(id) ? NoContent() : NotFound();
        

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)=> await _bookService.DeleteCategoryById(id)? NoContent():NotFound();




    }
}
