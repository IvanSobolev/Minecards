namespace Backend.DAO.Models;

public class Pack
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlImage { get; set; }
    
    public ICollection<CardBase> CardBases { get; set; }
}