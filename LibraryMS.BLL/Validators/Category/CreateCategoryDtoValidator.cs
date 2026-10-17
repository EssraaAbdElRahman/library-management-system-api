using LibraryMS.BLL.DTOs.Category;
namespace LibraryMS.BLL.Validators.Category
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(150);
        }
    }
}
