namespace LibraryMS.DAL.Repositories.Abstraction
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<Member?> GetByEmailAsync(string email);
        Task<bool> IsEmailExistsAsync(string email, int? excludeMemberId = null);
        Task<bool> IsPhoneExistsAsync(string phoneNumber, int? excludeMemberId = null);
        Task<Member?> GetByIdWithBorrowRecordsAsync(int id);
        Task<List<Member>> SearchAsync(string? name, bool? isActive, int page, int pageSize);
    }
}
