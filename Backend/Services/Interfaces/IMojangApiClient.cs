using Backend.DAO.DTOs.Mojang;

namespace Backend.Services.Interfaces;

public interface IMojangApiClient
{
    Task<MojangProfileResponse?> GetProfileByUsernameAsync(string username);
    Task<MojangSessionProfileResponse?> GetSessionProfileByUuidAsync(string uuid);
}