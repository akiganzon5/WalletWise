using System.ComponentModel.DataAnnotations;

namespace FinanceAPI.DTOs
{

    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }

}
