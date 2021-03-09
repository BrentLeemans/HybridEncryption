using HybridCrypto.Domain;

namespace HybridCrypto.Business
{
    public class EncryptedPacket
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }
        public byte[] EncryptedSessionKey { get; set; }
        public byte[] EncryptedData { get; set; }
        public byte[] Iv { get; set; }
        public byte[] Hmac { get; set; }
        public byte[] Signature { get; set; }
        public bool isFile { get; set; }
    }
}
