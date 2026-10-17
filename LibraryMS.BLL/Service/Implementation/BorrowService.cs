using LibraryMS.BLL.DTOs.BorrowRecord;
namespace LibraryMS.BLL.Service.Implementation
{
    public class BorrowService : IBorrowService
    {
        private const decimal FinePerDay = 5m; 

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BorrowService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BorrowRecordDto> GetByIdAsync(int id)
        {
            var record = await _unitOfWork.BorrowRecords.GetByIdWithDetailsAsync(id);
            if (record is null)
                throw new NotFoundException($"Borrow record with id {id} not found.");

            return _mapper.Map<BorrowRecordDto>(record);
        }

        public async Task<List<BorrowRecordDto>> GetByMemberAsync(int memberId)
        {
            var member = await _unitOfWork.Members.GetByIdWithBorrowRecordsAsync(memberId);
            if (member is null)
                throw new NotFoundException($"Member with id {memberId} not found.");

            return _mapper.Map<List<BorrowRecordDto>>(member.BorrowRecords);
        }

        public async Task<List<BorrowRecordDto>> GetOverdueAsync()
        {
            var records = await _unitOfWork.BorrowRecords.GetOverdueAsync();
            return _mapper.Map<List<BorrowRecordDto>>(records);
        }

        public async Task<BorrowRecordDto> BorrowBookAsync(CreateBorrowDto dto, string createdBy)
        {
             var book = await _unitOfWork.Books.GetByIdAsync(dto.BookId);
            if (book is null)
                throw new NotFoundException($"Book with id {dto.BookId} not found.");

             var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId);
            if (member is null)
                throw new NotFoundException($"Member with id {dto.MemberId} not found.");

            if (!member.IsActive)
                throw new BusinessRuleException("This member is not active and cannot borrow books.");

             var hasOverdue = await _unitOfWork.BorrowRecords.HasOverdueBooksAsync(dto.MemberId);
            if (hasOverdue)
                throw new BusinessRuleException("This member has overdue books and cannot borrow more.");

             book.BorrowCopy(createdBy);

             var borrowRecord = new BorrowRecord(dto.BookId, dto.MemberId, dto.DueDate, dto.BorrowNotes, createdBy);
            await _unitOfWork.BorrowRecords.AddAsync(borrowRecord);

             await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<BorrowRecordDto>(borrowRecord);
        }

        public async Task<BorrowRecordDto> ReturnBookAsync(int borrowRecordId, ReturnBookDto dto, string updatedBy)
        {
             var record = await _unitOfWork.BorrowRecords.GetByIdWithDetailsAsync(borrowRecordId);
            if (record is null)
                throw new NotFoundException($"Borrow record with id {borrowRecordId} not found.");

            if (record.IsReturned)
                throw new BusinessRuleException("This book has already been returned.");

            var returnDate = DateTime.Now;

             record.ReturnBook(returnDate, dto.ReturnNotes, updatedBy);

             record.Book!.ReturnCopy(updatedBy);

             if (returnDate > record.DueDate)
            {
                var daysLate = (returnDate.Date - record.DueDate.Date).Days;
                var fine = daysLate * FinePerDay;
                record.SetFine(fine, updatedBy);
            }

             await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<BorrowRecordDto>(record);
        }
    }
}
