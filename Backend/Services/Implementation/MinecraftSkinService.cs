using System.Text;
using System.Text.Json;
using Backend.DAO.Abstractions.Exceptions;
using Backend.DAO.DTOs.Mojang;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementation;

public class MinecraftSkinService(IMojangApiClient mojangApiClient) : IMinecraftSkinService
{
    private readonly IMojangApiClient _mojangApiClient = mojangApiClient;

    public async Task<string> GetSkinUrlByUsernameAsync(string username)
    {
        var profile = await _mojangApiClient.GetProfileByUsernameAsync(username);
        if (profile is null) throw new MojangPlayerNotFoundExeception(username);

        var sessionProfile = await _mojangApiClient.GetSessionProfileByUuidAsync(profile.Id);
        
        var textureProperty = sessionProfile?.Properties?.FirstOrDefault(p => p.Name == "textures");
        if (textureProperty is null) throw new MojangPlayerNotFoundExeception(username);

        var base64Data = Convert.FromBase64String(textureProperty.Value);
        var decodedJson = Encoding.UTF8.GetString(base64Data);
        
        var decodedTextures = JsonSerializer.Deserialize<DecodedTextures>(decodedJson);
        
        string? skinUrl = decodedTextures?.Textures?.Skin?.Url;
        if (string.IsNullOrEmpty(skinUrl))
        {
            throw new MojangPlayerNotFoundExeception(username);
        }

        return skinUrl;
    }
}