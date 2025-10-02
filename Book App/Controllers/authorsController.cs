using AutoMapper;
using Book_App.DTOs;
using Book_App.Exceptions;
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
        private readonly IAuthorService _authorService;

        public authorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetById(int id)
        {
            var authorDTO = await _authorService.GetById(id);
            if(authorDTO == null) { return NotFound(); }
            return Ok(authorDTO);
        }
        
        [HttpPost]
        public async Task<ActionResult<AuthorInfoDTO>> Create(CreateAuthorDTO authorDTO)
        {
            try
            {
                var author = await _authorService.Add(authorDTO);
                return CreatedAtAction(nameof(GetById), new { id = author.Id },
                author);
            }

            catch (NotFoundException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorInfoDTO>> Update(int id, CreateAuthorDTO author)
        {
            var updated = await _authorService.Update(id, author);
            if (updated == null) { return NotFound(); }
            return Ok(updated);

        }

    }
}
