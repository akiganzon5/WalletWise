using AutoMapper;
using FinanceAPI.DTOs;
using FinanceAPI.Model;
using FinanceAPI.Repository;
using System.Security.Claims;

namespace FinanceAPI.Services
{
    public class CategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CategoryService(
            IRepository<Category> categoryRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var userId = GetCurrentUserId();
            var categories = await _categoryRepository.FindAsync(c => c.UserId == userId);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var userId = GetCurrentUserId();
            var category = await _categoryRepository.FirstOrDefaultAsync(
                c => c.Id == id && c.UserId == userId
            );

            return category == null ? null : _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            category.UserId = GetCurrentUserId();

            await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var userId = GetCurrentUserId();
            var category = await _categoryRepository.FirstOrDefaultAsync(
                c => c.Id == id && c.UserId == userId
            );

            if (category == null)
                return false;

            _mapper.Map(dto, category);
            await _categoryRepository.UpdateAsync(category);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var userId = GetCurrentUserId();
            var category = await _categoryRepository.FirstOrDefaultAsync(
                c => c.Id == id && c.UserId == userId
            );

            if (category == null)
                return false;

            await _categoryRepository.DeleteAsync(category.Id);
            return true;
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

}
