using FluentValidation;

using System.Collections.Generic;

namespace backend.fluentvalidation.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>
            (this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count < num)
                .WithMessage("此列表包含过多项目！");
        }

        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainMoreThan<T, TElement>
            (this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count >= num)
                .WithMessage("此列表包含过少项目！");
        }
    }
}
