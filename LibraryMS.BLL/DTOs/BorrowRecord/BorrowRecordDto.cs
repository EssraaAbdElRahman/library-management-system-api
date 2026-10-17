namespace LibraryMS.BLL.DTOs.BorrowRecord
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public decimal FineAmount { get; set; }
        public string? BorrowNotes { get; set; }
        public string? ReturnNotes { get; set; }
    }
}
