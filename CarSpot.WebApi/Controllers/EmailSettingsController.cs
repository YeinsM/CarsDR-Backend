using System.Threading.Tasks;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmailSettingsController(IEmailSettingsRepository repository, IEmailService emailService) : ControllerBase
{
    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        EmailSettings? settings = await repository.GetSettingsAsync();
        if (settings == null)
        {
            return NotFound("Email settings not found.");
        }

        return Ok(settings);
    }

   
    
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] EmailSettings settings)
    {
        EmailSettings? existing = await repository.GetSettingsAsync();
        if (existing != null)
        {
            return BadRequest("Email settings already exist. Use PUT to update.");
        }

        await repository.AddAsync(settings);
        return CreatedAtAction(nameof(Get), new { id = settings.Id }, settings);
    }


    
    [HttpPut]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update([FromBody] EmailSettings updatedSettings)
    {
        bool updated = await repository.UpdateAsync(updatedSettings);
        if (!updated)
        {
            return NotFound("Email settings not found.");
        }

        return NoContent();
    }
}
