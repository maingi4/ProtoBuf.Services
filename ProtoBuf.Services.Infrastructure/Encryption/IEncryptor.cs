namespace ProtoBuf.Services.Infrastructure.Encryption
{
    internal interface IEncryptor
    {
        string Key { get; set; }

        string Encrypt(string plainText);

        string Decrypt(string encryptedText);
    }
}
