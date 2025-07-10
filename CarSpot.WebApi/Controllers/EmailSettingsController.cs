using System.Threading.Tasks;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmailSettingsController : ControllerBase
{
    private readonly IEmailSettingsRepository _repository;
    private readonly IEmailService _emailService;


    public EmailSettingsController(IEmailSettingsRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var settings = await _repository.GetSettingsAsync();
        if (settings == null)
            return NotFound("Email settings not found.");

        return Ok(settings);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmailSettings settings)
    {
        var existing = await _repository.GetSettingsAsync();
        if (existing != null)
            return BadRequest("Email settings already exist. Use PUT to update.");

        await _repository.AddAsync(settings);
        return CreatedAtAction(nameof(Get), new { id = settings.Id }, settings);
    }



    [HttpPut]
    public async Task<IActionResult> Update([FromBody] EmailSettings updatedSettings)
    {
        var updated = await _repository.UpdateAsync(updatedSettings);
        if (!updated)
            return NotFound("Email settings not found.");

        return NoContent();
    }

}
