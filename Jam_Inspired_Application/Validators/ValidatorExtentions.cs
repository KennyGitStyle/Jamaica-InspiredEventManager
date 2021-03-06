using FluentValidation;

namespace Jam_Inspired_Application.Validators
{
    public static class ValidatorExtentions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder){
            var options = ruleBuilder.NotEmpty().MinimumLength(8)
                .WithMessage("Password must have atleast 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain atleast 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain non aphanumeric character");

                return options;
        }
    }
}