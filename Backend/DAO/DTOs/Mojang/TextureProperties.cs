using System.Text.Json.Serialization;

namespace Backend.DAO.DTOs.Mojang;

public class TextureProperties
{
    [JsonPropertyName("SKIN")] public SkinInfo Skin { get; set; }
}