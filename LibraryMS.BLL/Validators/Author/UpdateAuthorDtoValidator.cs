using LibraryMS.BLL.DTOs.Author;
namespace LibraryMS.BLL.Validators.Author
{
    public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
    {
        public UpdateAuthorDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(150);

            RuleFor(x => x.Bio)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Bio));
        }
    }
}
