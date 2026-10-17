namespace LibraryMS.DAL.Entities
{
    public class BorrowRecord : BaseEntity
    {
        public int BookId { get; private set; }
        public Book? Book { get; private set; }
        public int MemberId { get; private set; }
        public Member? Member { get; private set; }
        public DateTime BorrowDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }
        public bool IsReturned { get; private set; } = false;
        public decimal FineAmount { get; private set; } = 0;
        public string? BorrowNotes { get; private set; }
        public string? ReturnNotes { get; private set; }
        private BorrowRecord() { }

        public BorrowRecord(
              int bookId,
              int memberId,
              DateTime dueDate,
              string? borrowNotes,
              string createdBy)
        {
            if (bookId <= 0)
                throw new ArgumentException("Book is required.");

            if (memberId <= 0)
                throw new ArgumentException("Member is required.");

            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("User is required.");

            BorrowDate = DateTime.Now;

            if (dueDate <= BorrowDate)
                throw new ArgumentException(
                    "Due date must be after the borrow date.");

            BookId = bookId;
            MemberId = memberId;
            DueDate = dueDate;
            FineAmount = 0;
            IsReturned = false;
            BorrowNotes = borrowNotes?.Trim();

            Created(createdBy.Trim());
        }

        public void ReturnBook(
            DateTime returnDate,
            string? returnNotes,
            string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");

            if (IsReturned)
                throw new InvalidOperationException(
                    "This book has already been returned.");

            if (returnDate < BorrowDate)
                throw new ArgumentException(
                    "Return date cannot be before borrow date.");

            ReturnDate = returnDate;
            ReturnNotes = returnNotes?.Trim();
            IsReturned = true;

            Updated(updatedBy.Trim());
        }

        public void SetFine(decimal fineAmount, string updatedBy)
        {
            if (fineAmount < 0)
                throw new ArgumentException(
                    "Fine amount cannot be negative.");

            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");

            FineAmount = fineAmount;

            Updated(updatedBy.Trim());
        }
    }
}