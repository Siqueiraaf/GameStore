using GameStore.PackingService.Core.Models;
using GameStore.PackingService.Features.DTOs;

namespace GameStore.PackingService.Features.Services.Interfaces;

public interface IPackService
{
    PackingDto PackOrder(Order order);
}
