using LibraryMS.BLL.DTOs.BorrowRecord;
namespace LibraryMS.BLL.Validators.BorrowRecord
{
    public class CreateBorrowDtoValidator : AbstractValidator<CreateBorrowDto>
    {
        public CreateBorrowDtoValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Book is required.");

            RuleFor(x => x.MemberId)
                .GreaterThan(0).WithMessage("Member is required.");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Due date must be in the future.");

            RuleFor(x => x.BorrowNotes)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.BorrowNotes));
        }
    }
}
