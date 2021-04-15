using FluentValidation;
using PlayerStatsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Models.Validators
{
    public class PlayerStatsQueryValidator : AbstractValidator<PlayerStatsQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15, 25, 50, 100 };
        private string[] allowedSortByColumnNames = { nameof(Game.Name), nameof(Game.Category), nameof(Game.PlayerStats) };
        public PlayerStatsQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value,context) =>
            {
                context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
            });
            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage( $"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
            
        }
    }
}
