using System.Text.Json.Serialization;

namespace TestApi;

public class MinecraftProfiles
{
    public string id { get; set; }
    public string name { get; set; }
}
public class ProfileProperty
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

// Основной класс для всего JSON-ответа
public class UserProfile
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    // "properties" - это массив объектов, поэтому используем List<T>
    [JsonPropertyName("properties")]
    public List<ProfileProperty> Properties { get; set; }
}

public class SkinInfo
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}

// Класс для объекта "textures", который может содержать скин (SKIN) и плащ (CAPE)
public class TextureProperties
{
    [JsonPropertyName("SKIN")]
    public SkinInfo Skin { get; set; }

    // Добавим и плащ, на случай если он есть. 
    // Если его не будет в JSON, это свойство просто останется null.
    [JsonPropertyName("CAPE")] 
    public SkinInfo Cape { get; set; }
}

// Главный класс для всего декодированного JSON
public class DecodedProfileTextures
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("profileId")]
    public string ProfileId { get; set; }

    [JsonPropertyName("profileName")]
    public string ProfileName { get; set; }

    [JsonPropertyName("textures")]
    public TextureProperties Textures { get; set; }
}