using LibraryMS.BLL.DTOs.Book;
namespace LibraryMS.BLL.Validators.Book
{
    public class AddBookCopiesDtoValidator : AbstractValidator<AddBookCopiesDto>
    {
        public AddBookCopiesDtoValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage("Count must be greater than zero.");
        }
    }
}
