using LibraryMS.BLL.DTOs.Member;

namespace LibraryMS.BLL.Mapping
{
    public class MemberProfile:Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberDto>()
                .ForMember(dest => dest.ActiveBorrowsCount,
                    opt => opt.MapFrom(src =>
                    src.BorrowRecords.Count(br => !br.IsReturned)));
               
        }
    }
}
