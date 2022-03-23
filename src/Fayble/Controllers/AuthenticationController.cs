using Fayble.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;


[Route("api/[controller]")]
[Authorize]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpPost]
    [Route("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<Authentication>> Refresh([FromBody] string refreshToken)
    {
        // var refreshResult = await _authenticationService.Refresh(refreshToken);

        // if (refreshResult != null)
        // {
        //    return refreshResult;
        // }

        return Unauthorized();
    }
}

