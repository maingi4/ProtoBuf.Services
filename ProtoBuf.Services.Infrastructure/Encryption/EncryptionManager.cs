using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Globalization;

namespace ProtoBuf.Services.Infrastructure.Encryption
{
    internal static class EncryptionManager
    {
        #region Initializers

        private static IEncryptor _encryptor;
        private static string _encryptionKey;
        private static readonly ConcurrentDictionary<string, string> EncryptionCache = new ConcurrentDictionary<string, string>();

        public static string EncryptionKey
        {
            get
            {
                return _encryptionKey;
            }
            set
            {
                _encryptionKey = value;
                EncryptionCache.Clear();
                if (string.IsNullOrWhiteSpace(value))
                {
                    _encryptor = new PlainTextEncryptor();
                }
                else
                {
                    _encryptor = new TripleDESEncryptor
                    {
                        Key = value
                    };
                }
            }
        }
        
        #endregion

        #region Public Methods

        public static string Encrypt(string plainText)
        {
            try
            {
                return EncryptionCache.GetOrAdd(string.Concat(plainText, "-en-"), s => _encryptor.Encrypt(plainText));
            }
            catch
            {
                EncryptionCache.Clear();
                throw;
            }
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                return EncryptionCache.GetOrAdd(string.Concat(encryptedText, "-dec-"),
                                                s => _encryptor.Decrypt(encryptedText));
            }
            catch
            {
                EncryptionCache.Clear();
                throw;
            }
        }
        
        #endregion
    }
}
