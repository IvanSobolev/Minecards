using Backend.DAO.DTOs.Pack;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacksController(IPackService packService, ILogger<PacksController> logger) : ControllerBase
{
    private readonly IPackService _packService = packService;
    private readonly ILogger<PacksController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PackDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PackDto>>> GetAllPacks()
    {
        _logger.LogInformation("Received a request to get all packs in the game");
        var packs = await _packService.GetAllPacksAsync();
        return Ok(packs);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(PackDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PackDto>> CreatePack([FromForm] CreatePackDto createPackDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _logger.LogInformation("A request was received to create a new pack named {PackName}", createPackDto.Name);
        try
        {
            var createdPack = await _packService.CreatePackAsync(createPackDto);
            _logger.LogInformation("Pack {PackName} with ID {PackId} was created successfully.", createdPack.Name, createdPack.Id);
            return CreatedAtAction(nameof(GetAllPacks), new { id = createdPack.Id }, createdPack);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Error creating pack: {ErrorMessage}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }
}