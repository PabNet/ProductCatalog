using System.Security.Cryptography;
using System.Text;

namespace ProductCatalog.Utility.Helpers
{
    public class EncryptionUtility
    {
        private readonly byte[] aesKey;
        public EncryptionUtility(ConfigurationUtility configurationUtility)
        {
            var encryptionKey = configurationUtility.GetValue("Encryption:Key");
            this.aesKey = HashStringKey(encryptionKey);
        }

        public string Encrypt(string value)
        {
            using (var aesAlg = new AesGcm(aesKey))
            {
                var nonce = new byte[12];
                var valueBytes = Encoding.UTF8.GetBytes(value);

                var (encryptedBytes, tag) = EncryptData(aesAlg, nonce, valueBytes);

                return Convert.ToBase64String(CombineEncryptedDataAndTag(encryptedBytes, tag));
            }
        }

        public string Decrypt(string encryptedValue)
        {
            using (var aesAlg = new AesGcm(aesKey))
            {
                var (encryptedBytes, tag) = SplitEncryptedValue(encryptedValue);
                var nonce = new byte[12];

                var decryptedBytes = DecryptData(aesAlg, nonce, encryptedBytes, tag);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        private byte[] HashStringKey(string key)
        {
            using (var sha256 = SHA256.Create())
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);
                return sha256.ComputeHash(keyBytes);
            }
        }

        private (byte[] EncryptedBytes, byte[] Tag) EncryptData(AesGcm aesAlg, byte[] nonce, byte[] valueBytes)
        {
            var encryptedBytes = new byte[valueBytes.Length];
            var additionalAuthenticatedData = new byte[0];
            var tag = new byte[16];

            aesAlg.Encrypt(nonce, valueBytes, encryptedBytes, tag, additionalAuthenticatedData);

            return (encryptedBytes, tag);
        }

        private byte[] DecryptData(AesGcm aesAlg, byte[] nonce, byte[] encryptedBytes, byte[] tag)
        {
            var additionalAuthenticatedData = new byte[0];
            var decryptedBytes = new byte[encryptedBytes.Length];

            aesAlg.Decrypt(nonce, encryptedBytes, tag, decryptedBytes, additionalAuthenticatedData);

            return decryptedBytes;
        }

        private byte[] CombineEncryptedDataAndTag(byte[] encryptedBytes, byte[] tag)
        {
            var result = new byte[encryptedBytes.Length + tag.Length];
            Buffer.BlockCopy(encryptedBytes, 0, result, 0, encryptedBytes.Length);
            Buffer.BlockCopy(tag, 0, result, encryptedBytes.Length, tag.Length);

            return result;
        }

        private (byte[] EncryptedBytes, byte[] Tag) SplitEncryptedValue(string encryptedValue)
        {
            var encryptedBytesWithTag = Convert.FromBase64String(encryptedValue);
            var encryptedBytes = new byte[encryptedBytesWithTag.Length - 16];
            var tag = new byte[16];

            Buffer.BlockCopy(encryptedBytesWithTag, 0, encryptedBytes, 0, encryptedBytes.Length);
            Buffer.BlockCopy(encryptedBytesWithTag, encryptedBytes.Length, tag, 0, 16);

            return (encryptedBytes, tag);
        }
    }
}
