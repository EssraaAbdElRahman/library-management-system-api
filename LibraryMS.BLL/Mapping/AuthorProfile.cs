using LibraryMS.BLL.DTOs.Author;

namespace LibraryMS.BLL.Mapping
{
    public class AuthorProfile:Profile 
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.BooksCount,
                    opt => opt.MapFrom(src => src.Books.Count));
        }
    }
}
