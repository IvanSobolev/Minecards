using Backend.DAO.DTOs.CardBase;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardBasesController(ICardBaseService cardBaseService, ILogger<CardBasesController> logger) : ControllerBase
{
    private readonly ICardBaseService _cardBaseService = cardBaseService;
    private readonly ILogger<CardBasesController> _logger = logger;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCardBase([FromForm] CreateCardBaseDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _logger.LogInformation("A request was received to create a card template {CardName} for pack {PackId}", dto.Name, dto.PackId);
        var newCardBaseId = await _cardBaseService.CreateCardBaseAsync(dto);

        return CreatedAtAction(null, new { id = newCardBaseId }, new { message = "Card base created successfully" });
    }
}