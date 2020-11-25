using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace SampleApi.Validators
{
    public interface IBaseValidator<in T> : IValidator<T>
    {
        Task ShallowValidateAndThrowAsync(T instance, CancellationToken cancellationToken = default, params string[] additionalRuleSets);
        Task ValidateAndThrowAsync(T instance, CancellationToken cancellationToken = default, params string[] additionalRuleSets);
    }
    
    public abstract class BaseValidator<T> : AbstractValidator<T>, IBaseValidator<T>
    {
        internal const string SHALLOW_RULE_SET = "shallow";
        internal const string DEFAULT_RULE_SET = "default";
        
        public Task ShallowValidateAndThrowAsync(T instance, CancellationToken cancellationToken = default, params string[] additionalRuleSets)
        {
            var ruleSets = new List<string> { SHALLOW_RULE_SET };
            ruleSets.AddRange(additionalRuleSets);
            return this.ValidateAndThrowAsync(instance, String.Join(',', ruleSets), cancellationToken);
        }
        
        public Task ValidateAndThrowAsync(T instance, CancellationToken cancellationToken = default, params string[] additionalRuleSets)
        {
            var ruleSets = new List<string> { DEFAULT_RULE_SET };
            ruleSets.AddRange(additionalRuleSets);
            return this.ValidateAndThrowAsync(instance, String.Join(',', ruleSets), cancellationToken);
        }
        
        protected IRuleBuilderInitial<T, TProperty> ShallowRuleFor<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            IRuleBuilderInitial<T, TProperty> result = null;

            RuleSet($"{DEFAULT_RULE_SET},{SHALLOW_RULE_SET}", () =>
            {
                result = RuleFor<TProperty>(expression);
            });

            return result;
        }

        public IRuleBuilderInitialCollection<T, TProperty> ShallowRuleForEach<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression)
        {
            IRuleBuilderInitialCollection<T, TProperty> result = null;

            RuleSet($"{DEFAULT_RULE_SET},{SHALLOW_RULE_SET}", () =>
            {
                result = RuleForEach<TProperty>(expression);
            });

            return result;
        }
        
        protected IRuleBuilderOptions<T, TProperty> ValidatorFor<TProperty>(Expression<Func<T, TProperty>> expression, IBaseValidator<TProperty> validator)
        {
            IRuleBuilderOptions<T, TProperty> result = null;

            RuleSet($"{DEFAULT_RULE_SET},{SHALLOW_RULE_SET}", () =>
            {
                result = RuleFor<TProperty>(expression).SetValidator(validator);
            });

            return result;
        }

        protected void ValidatorForEach<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression, IBaseValidator<TProperty> validator)
        {
            RuleSet($"{DEFAULT_RULE_SET},{SHALLOW_RULE_SET}", () =>
            {
                RuleForEach<TProperty>(expression).SetValidator(validator);
            });
        }
    }
}