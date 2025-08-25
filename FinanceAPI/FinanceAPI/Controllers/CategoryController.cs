using FinanceAPI.DTOs;
using FinanceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<ActionResult<CreateCategoryDto>> CreateCategory(CreateCategoryDto dto)
        {
            var category = await _categoryService.AddCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var category = await _categoryService.GetAllCategoriesAsync();
            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateCategoryDto>> UpdateCategory(int id, UpdateCategoryDto dto)
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, dto);
            if (updatedCategory == null)
                return NotFound();

            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deletedCategory = await _categoryService.DeleteCategoryAsync(id);
            if (!deletedCategory)
                return NotFound();

            return NoContent();
        }
    }
}
