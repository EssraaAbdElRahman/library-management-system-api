namespace LibraryMS.DAL.Repositories.Abstraction
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task DeleteAsync(int id, string deletedBy);
    }
}
