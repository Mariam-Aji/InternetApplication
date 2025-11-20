using FluentValidation;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Validation
{
    public class CreateComplaintRequestValidator : AbstractValidator<ComplaintRequest>
    {
        public CreateComplaintRequestValidator()
        {
            const string lettersOnlyRegex = @"^[\u0600-\u06FFa-zA-Z\s]+$";

            RuleFor(x => x.ComplaintType)
                .NotEmpty().WithMessage("نوع الشكوى مطلوب")
                .Matches(lettersOnlyRegex)
                .WithMessage("نوع الشكوى يجب أن يحتوي على أحرف عربية أو إنجليزية فقط");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("الموقع مطلوب")
                .Matches(lettersOnlyRegex)
                .WithMessage("الموقع يجب أن يحتوي على أحرف عربية أو إنجليزية فقط");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("الوصف مطلوب")
                .Matches(lettersOnlyRegex)
                .WithMessage("الوصف يجب أن يحتوي على أحرف عربية أو إنجليزية فقط");

            RuleFor(x => x.Image1)
                .NotNull().WithMessage("الصورة الأساسية Image1 مطلوبة");

            
            RuleFor(x => x.PdfFile)
                .NotNull().WithMessage("ملف الـ PDF مطلوب");

           

        }
    }
}
