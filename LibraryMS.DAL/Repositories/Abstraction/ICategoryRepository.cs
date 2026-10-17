namespace LibraryMS.DAL.Repositories.Abstraction
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> IsNameExistsAsync(string name, int? excludeCategoryId = null);
        Task<Category?> GetByIdWithBooksAsync(int id);
    }
}