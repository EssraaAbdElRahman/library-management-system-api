
namespace LibraryMS.DAL.Repositories.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(LibraryDbContext context) : base(context)
        {
        }

      
        public async Task<Category?> GetByIdWithBooksAsync(int id)
        {
            return await _dbSet
             .Include(c => c.Books)
             .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeCategoryId = null)
        {
            var query = _dbSet.Where(c => c.Name == name);
            if (excludeCategoryId.HasValue)
                query = query.Where(c => c.Id != excludeCategoryId.Value);

            return await query.AnyAsync();
        }
    }
}
