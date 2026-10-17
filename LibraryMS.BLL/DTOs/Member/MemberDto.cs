namespace LibraryMS.BLL.DTOs.Member
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime MembershipDate { get; set; }
        public bool IsActive { get; set; }
        public int ActiveBorrowsCount { get; set; }
    }
}
