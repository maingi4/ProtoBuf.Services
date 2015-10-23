namespace ProtoBuf.Services.Infrastructure.Encryption
{
    internal sealed class PlainTextEncryptor : IEncryptor
    {
        public string Key { get; set; }

        public string Encrypt(string plainText)
        {
            return plainText;
        }

        public string Decrypt(string encryptedText)
        {
            return encryptedText;
        }
    }
}