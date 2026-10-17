using LibraryMS.BLL.DTOs.Member;
namespace LibraryMS.BLL.Validators.Member
{
    public class UpdateMemberDtoValidator : AbstractValidator<UpdateMemberDto>
    {
        public UpdateMemberDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(200);

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^01[0125][0-9]{8}$").WithMessage("Invalid Egyptian phone number format.");
        }
    }
}
