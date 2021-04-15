using FluentValidation;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator(PlayerStatsDbContext dbContext)
        {
            RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();

            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.EmailAddress)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.User.Any(u => u.EmailAddress == value);
                    if (emailInUse)
                    {
                        context.AddFailure("EmailAdress", "That email is taken");
                    }
                });
        }
    }
}
