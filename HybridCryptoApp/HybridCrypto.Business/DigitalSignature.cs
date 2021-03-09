using System.Security.Cryptography;

namespace HybridCrypto.Business
{
    public static class DigitalSignature
    {
        public static RSACryptoServiceProvider AssignNewKey(string keyContainerName)
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
            return rsa;
        }

        public static RSACryptoServiceProvider GetKeyFromContainer(string keyContainerName)
        {
            var cspParameters = new CspParameters();
            cspParameters.KeyContainerName = keyContainerName;
            return new RSACryptoServiceProvider(4096, cspParameters);
        }

        public static byte[] SignData(byte[] hashOfDataToSign, string keyContainerName)
        {
            var rsa = GetKeyFromContainer(keyContainerName);

            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm("SHA256");

            return rsaFormatter.CreateSignature(hashOfDataToSign);
        }

        public static bool VerifySignature(byte[] hashOfDataToSign, byte[] signature, string keyContainerName)
        {
            var rsa = GetKeyFromContainer(keyContainerName);

            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm("SHA256");
            return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
        }
    }
}
