namespace LibraryMS.DAL.Repositories.Abstraction
{
    public interface IBorrowRecordRepository : IGenericRepository<BorrowRecord>
    {
        Task<BorrowRecord?> GetByIdWithDetailsAsync(int id);
        Task ReturnBookAsync(int id, DateTime? returnDate, string? returnNotes, string updatedBy);
        Task SetFineAsync(int id, decimal fineAmount, string updatedBy);
        Task<List<BorrowRecord>> GetOverdueAsync();
        Task<bool> HasOverdueBooksAsync(int memberId);
    }
}