using Backend.DAO.Enums;

namespace Backend.DAO.Models;

public class CardBase
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Creator { get; set; }
    public Rarity BaseRarityLevel { get; set; }
    public int PackId { get; set; }
    public string CardBgPhotoUrl { get; set; }
    public string SkinUrl { get; set; }
    
    public Pack Pack { get; set; }
    public ICollection<Card> Cards { get; set; }
}