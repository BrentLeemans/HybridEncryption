using HybridCrypto.Business;
using System;
using System.Collections.Generic;

namespace HybridCrypto.Domain
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public IList<EncryptedPacket> EncryptedPackets { get; set; }
        public virtual Sender Sender { get; set; }
        public virtual Receiver Receiver { get; set; }
        public DateTime Date { get; set; }
    }
}
