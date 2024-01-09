using FluentValidation;
using Shop.Application.SpecificationPatterns.Models;

namespace Shop.Application.SpecificationPatterns.Commands.EditSpecificationPattern;
public sealed class SimpleSpecificationPatternSpecificationTypeValidator : AbstractValidator<SimpleSpecificationPatternSpecificationTypeDto>
{
    public SimpleSpecificationPatternSpecificationTypeValidator()
    {
        RuleFor(x => x.SpecificationTypeId).NotEmpty();
    }
}
