using System.Net;
using Backend.DAO.DTOs.Mojang;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementation;

public class MojangApiClient(HttpClient httpClient, ILogger<MojangApiClient> logger) : IMojangApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<MojangApiClient> _logger = logger;

    public async Task<MojangProfileResponse?> GetProfileByUsernameAsync(string username)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<MojangProfileResponse>($"https://api.mojang.com/users/profiles/minecraft/{username}");
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NoContent || ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            _logger.LogError(ex, "An error occurred while requesting the Mojang profile for user {Username}. Status: {StatusCode}", username, ex.StatusCode);
            throw;
        }
    }

    public async Task<MojangSessionProfileResponse?> GetSessionProfileByUuidAsync(string uuid)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<MojangSessionProfileResponse>($"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}");
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NoContent || ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            _logger.LogError(ex, "An error occurred while requesting a Mojang session for UUID {Uuid}. Status: {StatusCode}", uuid, ex.StatusCode);
            throw;
        }
    }
}