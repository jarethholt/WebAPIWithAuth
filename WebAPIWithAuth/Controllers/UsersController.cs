using Microsoft.AspNetCore.Mvc;
using WebAPIWithAuth.Helpers;
using WebAPIWithAuth.Models;
using WebAPIWithAuth.Services;

namespace WebAPIWithAuth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest request)
    {
        var response = await _userService.Authenticate(request);
        if (response is null)
            return BadRequest(new { message = "Username or password is incorrect" });
        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] User user)
    {
        user.Id = 0;
        return Ok(await _userService.AddAndUpdateUser(user));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Put(int id, [FromBody] User user)
    {
        var currentUser = await _userService.GetUserById(id);
        if (currentUser is null)
            return BadRequest(new { message = $"User with id {id} does not exist" });
        return Ok(await _userService.AddAndUpdateUser(user));
    }
}
