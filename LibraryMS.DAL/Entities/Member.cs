namespace LibraryMS.DAL.Entities
{
    public class Member : BaseEntity
    {
        public string FullName { get; private set; } = string.Empty;
        public string? Email { get; private set; }
        public string PhoneNumber { get; private set; } = string.Empty;
        public DateTime MembershipDate { get; private set; }
        public bool IsActive { get; private set; } = true;

        // Navigation Property
        public ICollection<BorrowRecord> BorrowRecords { get; private set; } = new List<BorrowRecord>();
        private Member()
        {

        }
        public Member(string fullName, string? email, string phoneNumber,DateTime membershipDate, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Name for member is required.");
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone for member is required.");
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("User is required.");

            FullName = fullName.Trim();
            Email = email?.Trim();
            PhoneNumber = phoneNumber.Trim();
            MembershipDate = membershipDate;
            Created(createdBy.Trim());
        }
        public void UpdateMember(string fullName, string? email, string phoneNumber, string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Name for member is required.");
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone for member is required.");
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");

            FullName = fullName.Trim();
            Email = email?.Trim();
            PhoneNumber = phoneNumber.Trim();
            Updated(updatedBy.Trim());
        }
        public void Deactivate(string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");

            if (!IsActive)
                return;

            IsActive = false;
            Updated(updatedBy.Trim());
        }
        public void Activate(string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");

            IsActive = true;
            Updated(updatedBy.Trim());
        }
    }
}