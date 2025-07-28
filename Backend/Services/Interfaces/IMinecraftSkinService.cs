namespace Backend.Services.Interfaces;

public interface IMinecraftSkinService
{
    Task<string> GetSkinUrlByUsernameAsync(string username);
}