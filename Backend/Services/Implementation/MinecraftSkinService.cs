using System.Text;
using System.Text.Json;
using Backend.DAO.Abstractions.Exceptions;
using Backend.DAO.DTOs.Mojang;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementation;

public class MinecraftSkinService(IMojangApiClient mojangApiClient, ILogger<MinecraftSkinService> logger) : IMinecraftSkinService
{
    private readonly IMojangApiClient _mojangApiClient = mojangApiClient;
    private readonly ILogger<MinecraftSkinService> _logger = logger;

    public async Task<string> GetSkinUrlByUsernameAsync(string username)
    {
        _logger.LogDebug("Starting to get UUID for user {Username}", username);
        var profile = await _mojangApiClient.GetProfileByUsernameAsync(username);
        if (profile is null) throw new MojangPlayerNotFoundExeception(username);
        
        _logger.LogDebug("UUID {ProfileId} received for user {Username}",profile.Id, username );
        
        var sessionProfile = await _mojangApiClient.GetSessionProfileByUuidAsync(profile.Id);
        
        var textureProperty = sessionProfile?.Properties?.FirstOrDefault(p => p.Name == "textures");
        if (textureProperty is null)
        {
            _logger.LogWarning("User {Username} (UUID: {ProfileId} is missing texture properties in its profile.", username, profile.Id);
            throw new MojangPlayerNotFoundExeception(username);
        }
        
        _logger.LogDebug("Starting texture decoding for {Username}", username);
        
        var base64Data = Convert.FromBase64String(textureProperty.Value);
        var decodedJson = Encoding.UTF8.GetString(base64Data);
        
        var decodedTextures = JsonSerializer.Deserialize<DecodedTextures>(decodedJson);
        
        string? skinUrl = decodedTextures?.Textures?.Skin?.Url;
        if (string.IsNullOrEmpty(skinUrl))
        {
            _logger.LogWarning("The texture data for user {Username} is missing a skin URL.", username);
            throw new MojangPlayerNotFoundExeception(username);
        }

        return skinUrl;
    }
}