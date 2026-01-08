using FluentValidation;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Validation
{
    public class GovernmentAgencyValidator : AbstractValidator<GovernmentAgency>
    {
        public GovernmentAgencyValidator()
        {
            const string arabicEnglishRegex = @"^[\u0600-\u06FFa-zA-Z\s]+$";

            RuleFor(x => x.AgencyName)
                .NotEmpty().WithMessage("اسم الجهة مطلوب")
                .Matches(arabicEnglishRegex)
                    .WithMessage("اسم الجهة يجب أن يحتوي على أحرف عربية أو إنجليزية فقط");
        }
    }
}
