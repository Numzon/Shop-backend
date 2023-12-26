using FluentValidation;
using Shop.Application.SpecificationTypes.Models;

namespace Shop.Application.SpecificationTypes.Commands.CreateCategory;
public sealed class CreateSubspecificationTypeValidator : AbstractValidator<CreateSubspecificationTypeDto>
{
    public CreateSubspecificationTypeValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
