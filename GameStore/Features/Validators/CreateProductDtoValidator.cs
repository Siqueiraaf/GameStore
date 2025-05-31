using FluentValidation;
using GameStore.PackingService.Features.DTOs;

namespace GameStore.PackingService.Features.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("A altura deve ser maior que zero.");

        RuleFor(x => x.Width)
            .GreaterThan(0).WithMessage("A largura deve ser maior que zero.");

        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage("O comprimento deve ser maior que zero.");
    }
}
