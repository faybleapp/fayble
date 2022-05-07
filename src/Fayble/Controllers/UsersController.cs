using Fayble.Security.Authorisation;
using Fayble.Security.Models;
using Fayble.Security.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.User)]
public class UsersController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public UsersController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<User> Get(Guid id)
    {
        return await _authenticationService.GetUser(id);
    }


    [HttpGet]
    [Route("current")]
    public async Task<User> GetCurrentUser()
    {
        return await _authenticationService.GetCurrentUser();
    }

    [HttpPost]
    [Authorize(Policy = Policies.Administrator)]
    [Route("create-user")]
    public async Task<User> CreateUser([FromBody] NewUser newUser)
    {
        return await _authenticationService.CreateUser(newUser);
    }
}