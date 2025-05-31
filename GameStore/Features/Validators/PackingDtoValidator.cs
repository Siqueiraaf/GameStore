using FluentValidation;
using GameStore.PackingService.Features.DTOs;

namespace GameStore.PackingService.Features.Validators;

public class PackingDtoValidator : AbstractValidator<PackingDto>
{
    public PackingDtoValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0).WithMessage("O ID do pedido deve ser maior que zero.");

        RuleFor(x => x.UsedBoxesDto)
            .NotEmpty().WithMessage("A lista de caixas utilizadas não pode estar vazia.");

        RuleForEach(x => x.UsedBoxesDto)
            .SetValidator(new UsedBoxDtoValidator());
    }
}