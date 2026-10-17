using LibraryMS.BLL.DTOs.Book;
namespace LibraryMS.BLL.Service.Abstraction
{
    public interface IBookService
    {
        Task<BookDto> GetByIdAsync(int id);
        Task<List<BookDto>> SearchAsync(string? title, int? categoryId, int page, int pageSize);

        Task<BookDto> CreateAsync(CreateBookDto dto, string createdBy);
        Task<BookDto> UpdateAsync(int id, UpdateBookDto dto, string updatedBy);
        Task DeleteAsync(int id, string deletedBy);

        Task<BookDto> AddCopiesAsync(int id, AddBookCopiesDto dto, string updatedBy);
        Task<BookDto> RemoveCopiesAsync(int id, RemoveBookCopiesDto dto, string updatedBy);

        Task<BookDto> AddAuthorAsync(int id, AddAuthorToBookDto dto, string updatedBy);
        Task<BookDto> RemoveAuthorAsync(int id, int authorId, string updatedBy);
    }
}