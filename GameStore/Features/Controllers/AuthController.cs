using System.Net.Mime;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.PackingService.Features.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("/auth")]
[ApiController]
public class AuthController(IUserService userService, IAuthService authService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userService.ValidateUserAsync(dto);
        if (user is null)
            return Unauthorized("Credenciais inválidas");

        var token = _authService.GenerateJwtToken(user);
        return Ok(new TokenResponseDto { Token = token });
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
    {
        try
        {
            var user = await _userService.RegisterAsync(dto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
