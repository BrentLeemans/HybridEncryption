namespace HybridCryptoAPI
{
    public class TokenSettings
    {
        public string Key { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpirationTimeInMinutes { get; set; }
    }
}