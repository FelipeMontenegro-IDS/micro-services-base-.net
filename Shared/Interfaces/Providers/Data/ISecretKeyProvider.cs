namespace Shared.Interfaces.Providers.Data;

public interface ISecretKeyProvider
{
    string EncryptConnectionString(string plainText);
    string DecryptConnectionString(string cipherText);
    string EncryptPassword(string plainText);
    string DecryptPassword(string cipherText);
    string EncryptJwt(string plainText);
    string DecryptJwt(string cipherText);

}