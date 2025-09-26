using Book_App.DTOs;
using Book_App.Models;
using Book_App.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Book_App.Controllers
{
    [Route("api/books/[controller]")]
    [ApiController]
    public class authorsController : ControllerBase
    {
        //private readonly BookService _bookService;
        private readonly IAuthorService _authorService;

        public authorsController(IBookService bookService, IAuthorService authorService)
        {
          //  _bookService = bookService;
            _authorService = authorService;
        }
        // GET api/<authorsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorById(id);
            if (author == null) { return NotFound(); }
            else return Ok(new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name,
                Books = author.Books.Select(b =>
                    new BookInfoDTO { Id = b.Id, Title = b.Title, Price = b.Price }).ToList(),
            });
        }
        // POST api/<authorsController>
        [HttpPost]
        public async Task<ActionResult<AuthorInfoDTO>> CreateAuthor(CreateAuthorDTO author)
        {
            var newAuthor = new Author { Name = author.Name };
            var created = await _authorService.AddAuthor(newAuthor);
            if (created == null) { return NotFound(); }
            return CreatedAtAction(nameof(GetAuthorById), new { id = created.Id },
                new AuthorInfoDTO { Id=created.Id, Name=created.Name});
        }

        // PUT api/<authorsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorInfoDTO>> UpdateAuthor(int id, CreateAuthorDTO author)
        {
            var updated = await _authorService.UpdateAuthor(id, author);
            if (updated == null) { return NotFound(); }
            return Ok(new AuthorInfoDTO {Id=updated.Id, Name=updated.Name});

        }

        // DELETE api/<authorsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
