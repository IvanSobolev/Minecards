using Backend.DAO.Enums;

namespace Backend.DAO.DTOs.CardBase;

public class CreateCardBaseDto
{
    public string Name { get; set; }
    public string? Creator { get; set; }
    public Rarity BaseRarityLevel { get; set; }
    public int PackId { get; set; }
    public IFormFile SkinFile { get; set; }
    public IFormFile BackgroundImageFile { get; set; }
}