namespace Shared.Interfaces.Helpers;

public interface IEncryptionHelper
{
    string Encrypt(string plainText, string key);
    string Decrypt(string cipherText, string key);
}