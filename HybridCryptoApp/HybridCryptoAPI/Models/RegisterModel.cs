using System.ComponentModel.DataAnnotations;

namespace HybridCryptoAPI.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Nickname { get; set; }

        public override string ToString()
        {
            return $"Email: {Email}, Nickname: {Nickname}";
        }
    }
}