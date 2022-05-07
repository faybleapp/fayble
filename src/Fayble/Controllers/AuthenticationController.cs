using Fayble.Domain.Aggregates.RefreshToken;
using Fayble.Security;
using Fayble.Security.Models;
using Fayble.Security.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthenticationResult>> Login([FromBody] LoginCredentials login)
    {
       return await _authenticationService.Login(login);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult<AuthenticationResult>> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        return await _authenticationService.RefreshToken(refreshTokenRequest.RefreshToken);
    }
}

