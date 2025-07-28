using Backend.DAO.Abstractions.Exceptions;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardCreatorController(IMinecraftSkinService skinService, ILogger<CardCreatorController> logger) : ControllerBase
{
    private readonly IMinecraftSkinService _skinService = skinService;
    private readonly ILogger<CardCreatorController> _logger = logger;
    
    [HttpGet("skin/{username}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSkinUrl(string username)
    {
        try
        {
            var skinUrl = await _skinService.GetSkinUrlByUsernameAsync(username);
            return Ok(new { url = skinUrl });
        }
        catch (MojangPlayerNotFoundExeception ex)
        {
            _logger.LogInformation(ex.Message);
            return NotFound(new { message = ex.Message });
        }
    }
}