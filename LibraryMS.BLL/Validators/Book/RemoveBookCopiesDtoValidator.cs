using LibraryMS.BLL.DTOs.Book;

namespace LibraryMS.BLL.Validators.Book
{
    public class RemoveBookCopiesDtoValidator : AbstractValidator<RemoveBookCopiesDto>
    {
        public RemoveBookCopiesDtoValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage("Count must be greater than zero.");
        }
    }
}