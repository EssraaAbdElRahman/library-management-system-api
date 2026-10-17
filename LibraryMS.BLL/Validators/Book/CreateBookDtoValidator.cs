using LibraryMS.BLL.DTOs.Book;
namespace LibraryMS.BLL.Validators.Book
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150);

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Matches(@"^\d{10}(\d{3})?$").WithMessage("ISBN must be 10 or 13 digits.");

            RuleFor(x => x.PublishedYear)
                .InclusiveBetween(1500, DateTime.UtcNow.Year)
                .WithMessage($"Published year must be between 1500 and {DateTime.Now.Year}.");

            RuleFor(x => x.TotalCopies)
                .GreaterThan(0).WithMessage("Total copies must be greater than zero.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.");

            RuleFor(x => x.AuthorIds)
                .NotEmpty().WithMessage("At least one author is required.");
        }
    }
}