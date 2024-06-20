using System.Text.Json.Serialization;

namespace GameMasterHub.Infrastructure.Dto.Responses
{
    public class AuthResponse
    {

        [JsonPropertyName("token")] public string? Token { get; set; }
    }
}
