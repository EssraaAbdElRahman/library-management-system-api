namespace LibraryMS.DAL.Repositories.Implementation
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<Member?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeMemberId = null)
        {
            var query = _dbSet.Where(m => m.Email == email);

            if (excludeMemberId.HasValue)
                query = query.Where(m => m.Id != excludeMemberId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> IsPhoneExistsAsync(string phoneNumber, int? excludeMemberId = null)
        {
            var query = _dbSet.Where(m => m.PhoneNumber == phoneNumber);

            if (excludeMemberId.HasValue)
                query = query.Where(m => m.Id != excludeMemberId.Value);

            return await query.AnyAsync();
        }

        public async Task<Member?> GetByIdWithBorrowRecordsAsync(int id)
        {
            return await _dbSet
                .Include(m => m.BorrowRecords)
                    .ThenInclude(br => br.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Member>> SearchAsync(string? name, bool? isActive, int page, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.FullName.Contains(name));

            if (isActive.HasValue)
                query = query.Where(m => m.IsActive == isActive.Value);

            return await query
                .OrderBy(m => m.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}