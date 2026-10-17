using LibraryMS.BLL.DTOs.Category;
namespace LibraryMS.BLL.Service.Abstraction
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetByIdAsync(int id);
        Task<List<CategoryDto>> GetAllAsync();

        Task<CategoryDto> CreateAsync(CreateCategoryDto dto, string createdBy);
        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto, string updatedBy);
        Task DeleteAsync(int id, string deletedBy);
    }
}
