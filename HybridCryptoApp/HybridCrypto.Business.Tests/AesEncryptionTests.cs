using NUnit.Framework;
using System.Linq;

namespace HybridCrypto.Business.Tests
{
    public class AesEncryptionTests
    {
        EncryptedPacket encryptedPacket;
        byte[] sessionKey;

        [SetUp]
        public void Setup()
        {
            encryptedPacket = new EncryptedPacket { Iv = AesEncryption.GenerateRandomNumber(16) };
            sessionKey = AesEncryption.GenerateRandomNumber(32);
        }

        [Test]
        [TestCase(5)]
        public void ShouldReturnNumberOfRightLength(int length)
        {
            byte[] result = AesEncryption.GenerateRandomNumber(length);
            Assert.That(result.Length.Equals(length));
        }

       [Test]
        public void EncryptionShouldHappenRight()
        {
            byte[] dataToEncrypt = new byte[] { 69 };
            byte[] result = AesEncryption.Encrypt(dataToEncrypt, sessionKey, encryptedPacket.Iv);

            Assert.That(AesEncryption.Decrypt(result, sessionKey, encryptedPacket.Iv).SequenceEqual(dataToEncrypt));
        }


        

    }
}