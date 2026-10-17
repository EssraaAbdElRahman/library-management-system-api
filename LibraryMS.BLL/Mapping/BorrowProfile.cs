using LibraryMS.BLL.DTOs.BorrowRecord;
namespace LibraryMS.BLL.Mapping
{
    public class BorrowProfile : Profile
    {
        public BorrowProfile()
        {
            CreateMap<BorrowRecord, BorrowRecordDto>()
                .ForMember(dest => dest.BookTitle,
                    opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty))
                .ForMember(dest => dest.MemberName,
                    opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : string.Empty));
        }
    }
}