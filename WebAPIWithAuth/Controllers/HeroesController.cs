using Microsoft.AspNetCore.Mvc;
using WebAPIWithAuth.Models;
using WebAPIWithAuth.Services;

namespace WebAPIWithAuth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HeroesController(IHeroService service) : ControllerBase
{
    private readonly IHeroService _service = service;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Hero>>> Get([FromQuery] bool? isActive = null) =>
        Ok(await _service.GetAllAsync(isActive));

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Hero>> Get([FromRoute] int id)
    {
        var hero = await _service.GetByIdAsync(id);
        return (hero is null) ? NotFound() : Ok(hero);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Hero>> Post([FromBody] HeroDTO heroDTO)
    {
        var hero = await _service.AddAsync(heroDTO);
        return (hero is null) ? BadRequest() : CreatedAtAction(nameof(Post), hero);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Hero>> Put([FromRoute] int id, [FromBody] HeroDTO heroDTO)
    {
        var hero = await _service.UpdateAsync(id, heroDTO);
        return (hero is null) ? BadRequest() : Ok(hero);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await _service.DeleteByIdAsync(id);
        return result ? NoContent() : NotFound();
    }
}
