using Backend.DAO.Abstractions.Exceptions;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YouTubeController(IYouTubeThumbnailService thumbnailService, ILogger<YouTubeController> logger) : ControllerBase
{
    private readonly IYouTubeThumbnailService _thumbnailService = thumbnailService;
    private readonly ILogger<YouTubeController> _logger = logger;
    
    [HttpGet("thumbnail")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetThumbnail([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return BadRequest(new { message = "Параметр 'url' не может быть пустым." });
        }
        
        _logger.LogInformation("A request was received to get a preview for YouTube URL: {Url}", url);

        try
        {
            var thumbnailUrl = await _thumbnailService.GetThumbnailUrlAsync(url);
            _logger.LogInformation("Successfully found preview for URL: {Url}", url);
            return Ok(new { url = thumbnailUrl });
        }
        catch (InvalidYouTubeUrlException ex)
        {
            _logger.LogWarning(ex, "An invalid YouTube URL was passed.");
            return BadRequest(new { message = ex.Message });
        }
        catch (YouTubeVideoNotFoundException ex)
        {
            _logger.LogInformation(ex, "Video at the provided URL not found.");
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while processing the YouTube URL: {Url}", url);
            return StatusCode(500, new { message = "Произошла внутренняя ошибка сервера." });
        }
    }
}