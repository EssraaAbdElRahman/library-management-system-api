namespace LibraryMS.BLL.DTOs.Member
{
    public class UpdateMemberDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
