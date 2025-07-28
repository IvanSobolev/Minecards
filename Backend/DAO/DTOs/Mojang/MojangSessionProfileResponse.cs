using System.Text.Json.Serialization;

namespace Backend.DAO.DTOs.Mojang;

public class MojangSessionProfileResponse
{
    [JsonPropertyName("properties")] public List<ProfileProperty> Properties { get; set; }
}