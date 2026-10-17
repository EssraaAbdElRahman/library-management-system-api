using LibraryMS.BLL.DTOs.Book;
namespace LibraryMS.BLL.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.AuthorNames,
                    opt => opt.MapFrom(src => src.Authors.Select(a => a.Name).ToList()));
        }
    }
}
