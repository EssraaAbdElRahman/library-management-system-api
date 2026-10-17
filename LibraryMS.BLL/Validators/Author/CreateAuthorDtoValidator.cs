using LibraryMS.BLL.DTOs.Author;

namespace LibraryMS.BLL.Validators.Author
{
    public class CreateAuthorDtoValidator : AbstractValidator<CreateAuthorDto>
    {
        public CreateAuthorDtoValidator()
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
