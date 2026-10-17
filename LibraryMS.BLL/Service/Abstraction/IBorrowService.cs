using LibraryMS.BLL.DTOs.BorrowRecord;
namespace LibraryMS.BLL.Service.Abstraction
{
    public interface IBorrowService
    {
        Task<BorrowRecordDto> GetByIdAsync(int id);
        Task<List<BorrowRecordDto>> GetByMemberAsync(int memberId);
        Task<List<BorrowRecordDto>> GetOverdueAsync();

        Task<BorrowRecordDto> BorrowBookAsync(CreateBorrowDto dto, string createdBy);
        Task<BorrowRecordDto> ReturnBookAsync(int borrowRecordId, ReturnBookDto dto, string updatedBy);
    }
}
