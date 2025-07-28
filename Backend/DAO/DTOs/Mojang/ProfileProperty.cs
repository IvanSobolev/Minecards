using System.Text.Json.Serialization;

namespace Backend.DAO.DTOs.Mojang;

public class ProfileProperty
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("value")] public string Value { get; set; }
}