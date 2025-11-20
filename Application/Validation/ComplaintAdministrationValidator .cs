namespace WebAPI.Application.Validation
{
    using FluentValidation;
    using global::WebAPI.Domain.Entities;

    namespace WebAPI.Application.Validation
    {
        public class ComplaintAdministrationValidator : AbstractValidator<ComplaintAdministration>
        {
            public ComplaintAdministrationValidator()
            {
                RuleFor(x => x.ComplaintId)
                    .NotEmpty().WithMessage("الشكوى المرتبطة مطلوبة");

                RuleFor(x => x.GovernmentAgencyId)
                    .NotEmpty().WithMessage("جهة حكومية مطلوبة للإدارة");
            }
        }
    }

}
