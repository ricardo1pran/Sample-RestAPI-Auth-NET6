using System.ComponentModel.DataAnnotations;

namespace RicardoDevAPI.Helpers
{
    public class SignUp
    {
        [Required]
        public string User_id { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string Nickname { get;set; } = string.Empty;
    }
}
