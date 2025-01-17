using Microsoft.Extensions.Options;
using Shared.Configurations;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Data;

namespace Shared.Providers.Data;

public class SecretKeyProvider : ISecretKeyProvider
{
    private readonly IEncryptionHelper _encryptionHelper;
    private readonly EncryptionKey _encryptionKey;

    public SecretKeyProvider(IOptions<EncryptionKey> encryptionKey,IEncryptionHelper encryptionHelper)
    {
        _encryptionHelper = encryptionHelper ?? throw new ArgumentNullException(nameof(encryptionHelper));
        _encryptionKey = encryptionKey.Value ?? throw new ArgumentNullException(nameof(encryptionKey));
    }

    public string EncryptConnectionString(string plainText)
    {
        return _encryptionHelper.Encrypt(plainText, _encryptionKey.ConnectionStringKey);
    }

    public string DecryptConnectionString(string cipherText)
    {
        return _encryptionHelper.Decrypt(cipherText, _encryptionKey.ConnectionStringKey);
    }

    public string EncryptPassword(string plainText)
    {
        return _encryptionHelper.Encrypt(plainText, _encryptionKey.PasswordKey);
    }

    public string DecryptPassword(string cipherText)
    {
        return _encryptionHelper.Decrypt(cipherText, _encryptionKey.PasswordKey);
    }

    public string EncryptJwt(string plainText)
    {
        return _encryptionHelper.Encrypt(plainText, _encryptionKey.JwtKey);
    }

    public string DecryptJwt(string cipherText)
    {
        return _encryptionHelper.Decrypt(cipherText, _encryptionKey.JwtKey);
    }
}