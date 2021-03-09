using System.IO;
using System.Security.Cryptography;

namespace HybridCrypto.Business
{
    public static class AesEncryption
    {
        //Symetric encryption is fast and efficient
        //The problem with symetric encryption is sharing keys.
        public static byte[] GenerateRandomNumber(int length)
        {
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            var randomNumber = new byte[length];
            randomNumberGenerator.GetBytes(randomNumber);

            return randomNumber;
        }

        public static byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            //AES is a block cipher algorithm. It encrypts data in block units rather than a singly byte at a time.

            //Block cipher algorithms use the same encryption algorithm for each block, 
            //Because of this a block of plain text will always return the same ciphertext when encrypted with the same key and algorithm.

            var aes = new AesCryptoServiceProvider()
            {
                //Because this behaviour can be used to crack a cipher, cipher modes are used that modify the encryption process based on feedback from the earlier block encryptions.

                //Cipher block chaining (CBC) - Before each plain text block is encrypted, it is combined with the cipher text at the previous block with a bitwise exlusive or operation.
                //This ensures that if the plain text contains many identical blocks, 
                //they will each encrypt to a different ciphertext block. The initialization vector is combined with a first plain text block by a bitwise exclusive or operation before the block is encrypted.  
                Mode = CipherMode.CBC,
                //When the message is not long enough, padding will add bytes.
                Padding = PaddingMode.PKCS7,
                //Stores the encryption key prior to running the encrypt and decrypt operations.
                Key = key,
                //The initialization vector is used to store an arbitrary number than can be used along with a secret key for encryption.
                //This number is only employed once during the encryption session. 
                //Using a IV prevents repetition in the encryption. Making it more difficult for a hacker who is using a dictionary attack to find patterns to break the cipher.
                //The IV prevents the appearance of corresponding duplicate character sequences in the ciphertext. 
                //The initialization vector is made known to the destination computer to facilitate decryption of the data when it is received.
                //The IV does not have to be kept secret and can be transmitted and stored along with the message in the clear... 
                IV = iv
            };

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
            cryptoStream.FlushFinalBlock();

            return memoryStream.ToArray();
        }

        public static byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            var aes = new AesCryptoServiceProvider()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = key,
                IV = iv
            };

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

            cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
            cryptoStream.FlushFinalBlock();

            return memoryStream.ToArray();
        }
    }
}
