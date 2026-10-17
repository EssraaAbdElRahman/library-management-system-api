using LibraryMS.BLL.DTOs.Member;

namespace LibraryMS.BLL.Service.Abstraction
{
    public interface IMemberService
    {
        Task<MemberDto> GetByIdAsync(int id);
        Task<List<MemberDto>> SearchAsync(string? name, bool? isActive, int page, int pageSize);

        Task<MemberDto> CreateAsync(CreateMemberDto dto, string createdBy);
        Task<MemberDto> UpdateAsync(int id, UpdateMemberDto dto, string updatedBy);
        Task DeleteAsync(int id, string deletedBy);

        Task<MemberDto> ActivateAsync(int id, string updatedBy);
        Task<MemberDto> DeactivateAsync(int id, string updatedBy);
    }
}