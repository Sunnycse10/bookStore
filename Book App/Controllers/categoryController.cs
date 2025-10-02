using Book_App.DTOs;
using Book_App.Models;
using Book_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Book_App.Controllers
{
    [Route("api/books/[controller]")]
    [ApiController]
    public class categoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public categoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryWithBooksDTO>> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null) { return NotFound(); }
            return Ok(category);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryWithBooksDTO>>> GetAll()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Create(CreateCategoryDTO createCategoryDTO)
        {
            var created = await _categoryService.Add(createCategoryDTO);
            if (created == null) { return NotFound(); }
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id) => await _categoryService.Delete(id) ? NoContent() : NotFound();

    }
}
