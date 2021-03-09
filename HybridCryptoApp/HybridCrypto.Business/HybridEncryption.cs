using System.Security.Cryptography;

namespace HybridCrypto.Business
{
    public static class HybridEncryption
    {
        public static EncryptedPacket EncryptData(byte[] original, string rsaContainer, string signatureContainer)
        {
            //Generate our session key.
            var sessionKey = AesEncryption.GenerateRandomNumber(32);

            //Create the encrypted packet and generate the IV.
            var encryptedPacket = new EncryptedPacket { Iv = AesEncryption.GenerateRandomNumber(16) };

            //Encrypt our data with AES.
            encryptedPacket.EncryptedData = AesEncryption.Encrypt(original, sessionKey, encryptedPacket.Iv);

            //Encrypt the session key with RSA.
            encryptedPacket.EncryptedSessionKey = RSAWithCSPKey.EncryptData(sessionKey, rsaContainer);

            var hmac = new HMACSHA256(sessionKey);
            encryptedPacket.Hmac = hmac.ComputeHash(encryptedPacket.EncryptedData);
            encryptedPacket.Signature = DigitalSignature.SignData(encryptedPacket.Hmac, signatureContainer);

            return encryptedPacket;
        }

        public static byte[] DecryptData(EncryptedPacket encryptedPacket, string rsaContainer, string signatureContainer)
        {
            //Decrypt AES Key with RSA.
            var decryptedSessionKey = RSAWithCSPKey.DecryptData(encryptedPacket.EncryptedSessionKey, rsaContainer);

            var hmac = new HMACSHA256(decryptedSessionKey);
            var hmacToCheck = hmac.ComputeHash(encryptedPacket.EncryptedData);

            if (!Compare(encryptedPacket.Hmac, hmacToCheck))
            {
                throw new CryptographicException("HMAC for decryption does not match encrypted packet!");
            }

            if (!DigitalSignature.VerifySignature(encryptedPacket.Hmac, encryptedPacket.Signature, signatureContainer))
            {
                throw new CryptographicException("Digital signature can not be verified!");
            }

            //Decrypt our data with AES using the decrypted session key.
            var decryptedData = AesEncryption.Decrypt(encryptedPacket.EncryptedData, decryptedSessionKey, encryptedPacket.Iv);
            return decryptedData;
        }

        private static bool Compare(byte[] array1, byte[] array2)
        {
            var result = array1.Length == array2.Length;
            for (var i = 0; i < array1.Length && i < array2.Length; ++i)
            {
                result &= array1[i] == array2[i];
            }
            return result;
        }
    }
}
