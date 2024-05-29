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
    public async Task<ActionResult<List<Hero>>> Get([FromQuery] bool? isActive = null) =>
        Ok(await _service.GetAllAsync(isActive));

    [HttpGet("{id}")]
    public async Task<ActionResult<Hero>> Get([FromRoute] int id)
    {
        var hero = await _service.GetByIdAsync(id);
        return (hero is null) ? NotFound() : Ok(hero);
    }

    [HttpPost]
    public async Task<ActionResult<Hero>> Post([FromBody] HeroDTO heroDTO)
    {
        var hero = await _service.AddAsync(heroDTO);
        return (hero is null) ? BadRequest() : CreatedAtAction(nameof(Post), hero);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Hero>> Put([FromRoute] int id, [FromBody] HeroDTO heroDTO)
    {
        var hero = await _service.UpdateAsync(id, heroDTO);
        return (hero is null) ? BadRequest() : Ok(hero);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await _service.DeleteByIdAsync(id);
        return result ? NoContent() : NotFound();
    }
}
