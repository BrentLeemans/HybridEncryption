using NUnit.Framework;
using System;
using System.Linq;

namespace HybridCrypto.Business.Tests
{
    public class HybridEncryptionTests
    {
        
        [SetUp]
        public void Setup()
        {
          
        }

        [TestCase(new byte[] {69}, "RSA_userID_82d8bece-0c68-450b-93fd-5b51cafbfe8d", "Signature_userID_80c905ab-860b-48c7-8e35-6750dd6d6fc9")]
       public void EncryptionShouldHappenRight(byte[] original, string rsaContainer, string signatureContainer)
        {
            EncryptedPacket encryptedPacket = HybridEncryption.EncryptData(original, rsaContainer, signatureContainer);
            Assert.That(HybridEncryption.DecryptData(encryptedPacket, rsaContainer, signatureContainer).SequenceEqual(original));
        }
}
    
   
    
}
