namespace LibraryMS.DAL.Repositories.Implementation
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<List<Author>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(a => a.Name.Contains(name))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<Author?> GetByIdWithBooksAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<List<Author>> GetAllWithBooksAsync()
        {
            return await _dbSet.Include(a => a.Books).ToListAsync();
        }
    }
}
