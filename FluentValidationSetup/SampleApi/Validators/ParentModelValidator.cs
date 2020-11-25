using FluentValidation;
using SampleApi.Models;

namespace SampleApi.Validators
{
    public class ParentModelValidator: BaseValidator<ParentModel>
    {
        public ParentModelValidator(IBaseValidator<ChildModel> childValidator)
        {
            ShallowRuleFor(x => x.FirstName)
                .NotEmpty();
            ShallowRuleFor(x => x.LastName)
                .NotEmpty();
            
            ValidatorFor(x => x.MainChild, childValidator);
            
            ValidatorForEach(x => x.Children, childValidator);
        }
    }
}