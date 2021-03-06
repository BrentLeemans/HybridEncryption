using System.ComponentModel.DataAnnotations;

namespace HybridCryptoAPI.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public override string ToString()
        {
            return $"Email: {Email}";
        }
    }
}