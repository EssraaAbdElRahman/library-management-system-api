using LibraryMS.BLL.DTOs.Category;

namespace LibraryMS.BLL.Mapping
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
              .ForMember(dest => dest.BooksCount,
                  opt => opt.MapFrom(src => src.Books.Count));
        }
    }
}
