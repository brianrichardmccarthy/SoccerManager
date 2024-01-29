using FluentValidation;
using SoccerManager.Models;

namespace SoccerManager.Validators;
/// <summary>
/// Basic create validator for Player class
/// </summary>
public class CreateValidator : AbstractValidator<Player>
{
    public CreateValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Position).NotEmpty().IsInEnum().WithMessage("Position is required and must be between 1 and 4 inclusive");
        RuleFor(p => p.SkillRating).InclusiveBetween(1, 100).WithMessage("Skill Rating must be between 1 and 100");
    }
}
