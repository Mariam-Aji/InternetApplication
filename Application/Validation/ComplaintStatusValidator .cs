using FluentValidation;
using WebAPI.Domain.Entities;

namespace WebAPI.Application.Validation
{
    public class ComplaintStatusValidator : AbstractValidator<ComplaintStatus>
    {
        public ComplaintStatusValidator()
        {
            const string arabicOnlyRegex = @"^[\u0600-\u06FF\s]+$";

            RuleFor(x => x.StatusName)
                .NotEmpty().WithMessage("حالة الشكوى مطلوبة")
                .Matches(arabicOnlyRegex)
                    .WithMessage("حالة الشكوى يجب أن تحتوي على أحرف عربية فقط");
        }
    }
}
