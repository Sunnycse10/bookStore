using AutoMapper;
using Book_App.DTOs;
using Book_App.Exceptions;
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

        public booksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAll()
        {
            var books = await _bookService.GetAll();
            return Ok(books);
        }

              

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetById(int id)
        {
            
            var book = await _bookService.GetById(id);
            return book == null ? NotFound():  Ok(book);

        }
        [HttpPost]
        public async Task<ActionResult<BookDTO>> Create(CreateBookDTO bookDto)
        {
            
            var created = await _bookService.Add(bookDto);
            if (created == null) { return NotFound(); }

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        
        
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDTO>> UpdateBook(int id, CreateBookDTO book)
        {            
            var updated = await _bookService.Update(id,book);
            return Ok(updated);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => await _bookService.Delete(id) ? NoContent() : NotFound();
        





    }
}
