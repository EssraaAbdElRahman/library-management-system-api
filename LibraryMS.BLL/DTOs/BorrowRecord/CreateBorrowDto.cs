namespace LibraryMS.BLL.DTOs.BorrowRecord
{
    public class CreateBorrowDto
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime DueDate { get; set; }
        public string? BorrowNotes { get; set; }
    }
}
