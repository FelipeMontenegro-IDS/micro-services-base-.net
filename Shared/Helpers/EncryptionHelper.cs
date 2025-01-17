using System.Security.Cryptography;
using System.Text;
using Shared.Interfaces.Helpers;
using Shared.RegularExpressions;

namespace Shared.Helpers;

public class EncryptionHelper : IEncryptionHelper
{
    public string Encrypt(string plainText, string key)
    {
        ValidationParamsEncrypt(plainText, key);

        using Aes aes = Aes.Create();

        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.GenerateIV();

        using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using MemoryStream ms = new MemoryStream();

        // Escribir el IV al principio del flujo
        ms.Write(aes.IV, 0, aes.IV.Length);

        using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using StreamWriter sw = new StreamWriter(cs);

        sw.Write(plainText);
        sw.Flush();
        cs.FlushFinalBlock();

        return Convert.ToBase64String(ms.ToArray());
    }
    
    public string Decrypt(string cipherText, string key)
    {
        ValidationParamsEncrypt(cipherText, key);
        
        var fullCipher = Convert.FromBase64String(cipherText);

        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);

        // Extraer el IV del texto cifrado
        var iv = new byte[aes.IV.Length];
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);

        using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, iv);
        using MemoryStream ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
        using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
    
    private static void ValidationParamsEncrypt(string plainText, string key)
    {
        if (string.IsNullOrWhiteSpace(plainText))
        {
            throw new ArgumentNullException(nameof(plainText), "The plain text cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key), "The key cannot be null or empty.");
        }

        if (!RegularExpression.AesKeyLength.IsMatch(key))
        {
            throw new ArgumentException("The key length is not valid.", nameof(key));
        }
    }

}