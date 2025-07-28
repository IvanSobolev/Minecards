using Backend.DAO.DTOs.Mojang;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementation;

public class MojangApiClient(HttpClient httpClient) : IMojangApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<MojangProfileResponse?> GetProfileByUsernameAsync(string username)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<MojangProfileResponse>($"https://api.mojang.com/users/profiles/minecraft/{username}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return null; // Игрок не найден
        }
    }

    public async Task<MojangSessionProfileResponse?> GetSessionProfileByUuidAsync(string uuid)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<MojangSessionProfileResponse>($"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return null;
        }
    }
}