using System.Text.Json.Serialization;

namespace Backend.DAO.DTOs.Mojang;

public class MojangProfileResponse
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}