using System;
using System.ComponentModel.DataAnnotations;

namespace HybridCryptoAPI.Models
{
    public class MessageModel
    {
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public byte[] File { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"SenderId: {SenderId}, ReceiverId {ReceiverId}, Date: {Date}";
        }
    }
}
