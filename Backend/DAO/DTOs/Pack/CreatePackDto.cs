namespace Backend.DAO.DTOs.Pack;

public class CreatePackDto
{
    public string Name { get; set; }
    public IFormFile ImageFile { get; set; }
}