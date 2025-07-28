using System.Text.Json.Serialization;

namespace Backend.DAO.DTOs.Mojang;

public class DecodedTextures
{
    [JsonPropertyName("textures")] public TextureProperties Textures { get; set; }
}