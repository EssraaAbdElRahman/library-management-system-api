using LibraryMS.BLL.DTOs.Book;

namespace LibraryMS.BLL.Validators.Book
{
    public class AddAuthorToBookDtoValidator : AbstractValidator<AddAuthorToBookDto>
    {
        public AddAuthorToBookDtoValidator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Author is required.");
        }
    }
}
