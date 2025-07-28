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
        _logger.LogInformation("A request was received to receive a skin for the user {Username}", username);
        try
        {
            var skinUrl = await _skinService.GetSkinUrlByUsernameAsync(username);
            
            _logger.LogInformation("Successfully found skin URL for user {Username}", username);
            return Ok(new { url = skinUrl });
        }
        catch (MojangPlayerNotFoundExeception ex)
        {
            _logger.LogInformation(ex.Message);
            
            _logger.LogInformation("User {Username} not found. Reason: {ExceptionMessage}", username, ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving the skin for user {Username}", username);
            return StatusCode(500, new { message = "Произошла внутренняя ошибка сервера." });
        }
    }
}