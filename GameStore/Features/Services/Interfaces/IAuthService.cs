using GameStore.PackingService.Core.Models;

namespace GameStore.PackingService.Features.Services.Interfaces;

public interface IAuthService
{
    string GenerateJwtToken(User user);
}
