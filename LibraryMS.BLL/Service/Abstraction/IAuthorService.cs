using LibraryMS.BLL.DTOs.Author;

namespace LibraryMS.BLL.Service.Abstraction
{
    public interface IAuthorService
    {
        Task<AuthorDto> GetByIdAsync(int id);
        Task<List<AuthorDto>> GetAllAsync();
        Task<List<AuthorDto>> SearchByNameAsync(string name);

        Task<AuthorDto> CreateAsync(CreateAuthorDto dto, string createdBy);
        Task<AuthorDto> UpdateAsync(int id, UpdateAuthorDto dto, string updatedBy);
        Task DeleteAsync(int id, string deletedBy);
    }
}