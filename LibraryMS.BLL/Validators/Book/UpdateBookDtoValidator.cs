using LibraryMS.BLL.DTOs.Book;

namespace LibraryMS.BLL.Validators.Book
{
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150);

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Matches(@"^\d{10}(\d{3})?$").WithMessage("ISBN must be 10 or 13 digits.");

            RuleFor(x => x.PublishedYear)
                .InclusiveBetween(1500, DateTime.UtcNow.Year)
                .WithMessage($"Published year must be between 1500 and {DateTime.UtcNow.Year}.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.");
        }
    }
}
