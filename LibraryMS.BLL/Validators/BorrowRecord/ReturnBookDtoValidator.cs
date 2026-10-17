using LibraryMS.BLL.DTOs.BorrowRecord;
namespace LibraryMS.BLL.Validators.BorrowRecord
{
    public class ReturnBookDtoValidator : AbstractValidator<ReturnBookDto>
    {
        public ReturnBookDtoValidator()
        {
            RuleFor(x => x.ReturnNotes)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.ReturnNotes));
        }
    }
}
