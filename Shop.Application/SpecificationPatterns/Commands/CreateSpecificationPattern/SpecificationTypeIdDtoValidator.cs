using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shop.Application.SpecificationPatterns.Models;

namespace Shop.Application.SpecificationPatterns.Commands.CreateSpecificationPattern;
public sealed class SpecificationTypeIdDtoValidator : AbstractValidator<SpecificationTypeIdDto>
{
    public SpecificationTypeIdDtoValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
