using FluentValidation;
using GameStore.PackingService.Features.DTOs;

namespace GameStore.PackingService.Features.Validators;

public class UsedBoxDtoValidator : AbstractValidator<UsedBoxDto>
{
    public UsedBoxDtoValidator()
    {
        RuleFor(x => x.BoxType)
            .NotEmpty().WithMessage("O tipo da caixa é obrigatório.");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("Cada caixa deve conter pelo menos um produto.");
    }
}
