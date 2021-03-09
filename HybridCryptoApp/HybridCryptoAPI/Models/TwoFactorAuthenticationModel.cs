using System;
using System.ComponentModel.DataAnnotations;

namespace HybridCryptoAPI.Models
{
    public class TwoFactorAuthenticationModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public int Code { get; set; }
        [Required]
        public Guid Guid { get; set; }
    }
}
