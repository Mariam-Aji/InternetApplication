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
                RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("حقل الملاحظات مطلوب")
                .MaximumLength(500).WithMessage("الملاحظات يجب ألا تتجاوز 500 حرفًا");

            }
        }
    }

}
