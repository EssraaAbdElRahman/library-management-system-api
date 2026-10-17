namespace LibraryMS.DAL.Repositories.Abstraction
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<List<Author>> SearchByNameAsync(string name);
        Task<Author?> GetByIdWithBooksAsync(int id);
        Task<List<Author>> GetAllWithBooksAsync();
    }
}
