using System.Security.Cryptography;
using System.Text;
using Shared.Interfaces.Helpers;

namespace Shared.Helpers;

public class EncryptionHelper : IEncryptionHelper
{
    public string Encrypt(string plainText, string key)
    {
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
}