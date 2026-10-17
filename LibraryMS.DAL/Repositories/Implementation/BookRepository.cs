namespace LibraryMS.DAL.Repositories.Implementation
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<Book?> GetByIsbnAsync(string isbn)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<Book?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(b => b.Category)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Book>> SearchAsync(string? title, int? categoryId, int page, int pageSize)
        {
            var query = _dbSet.Include(b => b.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(b => b.Title.Contains(title));

            if (categoryId.HasValue)
                query = query.Where(b => b.CategoryId == categoryId.Value);

            return await query
                .OrderBy(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> IsIsbnExistsAsync(string isbn, int? excludeBookId = null)
        {
            var query = _dbSet.Where(b => b.ISBN == isbn);

            if (excludeBookId.HasValue)
                query = query.Where(b => b.Id != excludeBookId.Value);

            return await query.AnyAsync();
        }
    }
}
