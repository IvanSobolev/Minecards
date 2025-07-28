using System.Text.Json.Serialization;

namespace Backend.DAO.DTOs.Mojang;

public class SkinInfo
{
    [JsonPropertyName("url")] public string Url { get; set; }
}