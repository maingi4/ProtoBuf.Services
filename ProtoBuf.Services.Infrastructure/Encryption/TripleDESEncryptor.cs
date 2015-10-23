using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace ProtoBuf.Services.Infrastructure.Encryption
{
    internal sealed class TripleDESEncryptor : IEncryptor
    {
        #region Exception Messages

        private const string SomethingWentWrong = "Something went wrong while encrypting/decrypting, please check logs.";

        #endregion

        #region IEncryptor Members

        public string Key { get; set; }

        public string Encrypt(string plainText)
        {
            try
            {
                return TripleDESEncrypt(plainText, Key);
            }
            catch(Exception ex)
            {
                throw new SecurityException("Unable to encrypt given text, check inner exception for details", ex);
            }
        }

        public string Decrypt(string encryptedText)
        {
            try
            {
                return TripleDESDecrypt(encryptedText, Key);
            }
            catch (Exception ex)
            {
                throw new SecurityException("Unable to decrypt given text, check inner exception for details", ex);
            }
        }

        #endregion

        #region Private Members

        private static string TripleDESEncrypt(string toEncrypt, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (toEncrypt == null)
                return null;

            var bytes1 = Encoding.UTF8.GetBytes(toEncrypt);
            var bytes2 = Encoding.UTF8.GetBytes(key);

            byte[] inArray;
            using (var cryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                cryptoServiceProvider.Key = bytes2;
                cryptoServiceProvider.Mode = CipherMode.ECB;
                cryptoServiceProvider.Padding = PaddingMode.PKCS7;
                inArray = cryptoServiceProvider.CreateEncryptor().TransformFinalBlock(bytes1, 0, bytes1.Length);
            }
            return Convert.ToBase64String(inArray, 0, inArray.Length);
        }

        private static string TripleDESDecrypt(string toDecrypt, string key)
        {
            var inputBuffer = Convert.FromBase64String(toDecrypt);
            var bytes1 = Encoding.UTF8.GetBytes(key);
            byte[] bytes2;
            using (var cryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                cryptoServiceProvider.Key = bytes1;
                cryptoServiceProvider.Mode = CipherMode.ECB;
                cryptoServiceProvider.Padding = PaddingMode.PKCS7;
                bytes2 = cryptoServiceProvider.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            }
            return Encoding.UTF8.GetString(bytes2);
        }

        #endregion
    }
}
