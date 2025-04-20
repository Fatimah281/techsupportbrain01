
using System.Text.Json.Serialization;

namespace TechSupportBrain.Models
{
    public class User
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("uid")]
        public string Uid { get; set; }
    }
}