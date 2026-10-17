namespace LibraryMS.DAL.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly LibraryDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(LibraryDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task DeleteAsync(int id, string deletedBy)
        {
            var entity = await GetByIdOrThrowAsync(id);
            entity.Deleted(deletedBy);
        }
        protected async Task<T> GetByIdOrThrowAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null)
                throw new NotFoundException($"{typeof(T).Name} with id {id} not found.");
            return entity;
        }
    }
}