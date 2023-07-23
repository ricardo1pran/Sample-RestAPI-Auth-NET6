using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RicardoDevAPI
{
    public class UserDTO
    {
        [Key]
        public string User_id { get; set; } = string.Empty;
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;

        public string Nickname { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }
}
