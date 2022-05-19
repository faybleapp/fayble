using Fayble.Models;
using Fayble.Security.Authorisation;
using Fayble.Services.Person;
using Fayble.Services.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.User)]
public class PeopleController : ControllerBase
{
    private readonly IPersonService _personService;

    public PeopleController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<IEnumerable<Person>?> GetAll()
    {
        return await _personService.GetPeople();
    }
}