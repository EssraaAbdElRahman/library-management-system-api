namespace LibraryMS.DAL.Repositories.Implementation
{
    public class BorrowRecordRepository : GenericRepository<BorrowRecord>, IBorrowRecordRepository
    {
        public BorrowRecordRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<BorrowRecord?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(br => br.Book)
                .Include(br => br.Member)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task ReturnBookAsync(int id, DateTime? returnDate, string? returnNotes, string updatedBy)
        {
            var result = await GetByIdOrThrowAsync(id); 
            result.ReturnBook(returnDate ?? DateTime.Now, returnNotes, updatedBy);
        }

        public async Task SetFineAsync(int id, decimal fineAmount, string updatedBy)
        {
            var result = await GetByIdOrThrowAsync(id); 
            result.SetFine(fineAmount, updatedBy);
        }
        // BorrowRecordRepository
        public async Task<List<BorrowRecord>> GetOverdueAsync()
        {
            return await _dbSet
                .Include(br => br.Book)
                .Include(br => br.Member)
                .Where(br => !br.IsReturned && br.DueDate < DateTime.Now)
                .OrderBy(br => br.DueDate)
                .ToListAsync();
        }

        public async Task<bool> HasOverdueBooksAsync(int memberId)
        {
            return await _dbSet
                .AnyAsync(br => br.MemberId == memberId
                             && !br.IsReturned
                             && br.DueDate < DateTime.Now);
        }
    }
}