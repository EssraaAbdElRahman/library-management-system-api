namespace LibraryMS.DAL.Repositories.Abstraction
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<Book?> GetByIsbnAsync(string isbn);
        Task<Book?> GetByIdWithDetailsAsync(int id);
        Task<List<Book>> SearchAsync(string? title, int? categoryId, int page, int pageSize);
        Task<bool> IsIsbnExistsAsync(string isbn, int? excludeBookId = null);
    }
}
