using System.Security.Cryptography;

namespace HybridCrypto.Business
{
    //Asymmetric encryption is a lot slower than symmetric encryption
    //Asymmetric key sharing is a better solution
    public static class RSAWithCSPKey
    {
        public static void AssignNewKey(string keyContainerName)
        {
            CspParameters cspParameters = new CspParameters(1)
            {
                KeyContainerName = keyContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore,
                ProviderName = "Microsoft Strong Cryptographic Provider"
            };
            var rsa = new RSACryptoServiceProvider(4096, cspParameters);
            //This means that we will store the key in the key container
            rsa.PersistKeyInCsp = true;
        }

        public static RSACryptoServiceProvider GetKeyFromContainer(string keyContainerName)
        { 
            var cspParameters = new CspParameters();
            cspParameters.KeyContainerName = keyContainerName;
            return new RSACryptoServiceProvider(4096, cspParameters);
        }

        //Verwijder key uit key container
        public static void DeleteKeyInCsp(string keyContainerName)
        {
            var rsa = GetKeyFromContainer(keyContainerName);
            rsa.PersistKeyInCsp = false;
            //This will flush the key from the key container
            rsa.Clear();
        }

        public static byte[] EncryptData(byte[] dataToEncrypt, string keyContainerName)
        {
            var rsa = GetKeyFromContainer(keyContainerName);
            //Tweede parameter is voor OAEP padding.
            //Optimal Asymmetric Encryption Padding
            //OAEP is a form of Feistel network, which is designed to add an element of randomness to the encryption process.
            //This helps prevent partial decryption of ciphertext by ensuring an attacker cannot recover any portion of the plain text.
            byte[] cipherbytes = rsa.Encrypt(dataToEncrypt, true);

            return cipherbytes;
        }

        public static byte[] DecryptData(byte[] dataToDecrypt, string keyContainerName)
        {
            var rsa = GetKeyFromContainer(keyContainerName);
            byte[] plain = rsa.Decrypt(dataToDecrypt, true);

            return plain;
        }
    }
}
