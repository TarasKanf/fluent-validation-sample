using System.Threading.Tasks;
using FluentValidation;
using SampleApi.Models;

namespace SampleApi.Validators
{
    public class ChildModelValidator: BaseValidator<ChildModel>
    {
        public ChildModelValidator()
        {
            ShallowRuleFor(x => x.FirstName)
                .NotEmpty();
            ShallowRuleFor(x => x.LastName)
                .NotEmpty();
            ShallowRuleFor(x => x.Age)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.FirstName)
                .MustAsync((model, nameof, ct) => {
                    // fake call to external resources that indicates that this model is invalid
                    return Task.FromResult<bool>(false);
                })
                .WithMessage("FirstName did not pass validation by some external resource. TEST");
        }
    }
}