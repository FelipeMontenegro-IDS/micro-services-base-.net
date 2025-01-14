using System.Security.Cryptography;
using System.Text;
using Shared.Interfaces.Helpers;

namespace Shared.Helpers;

public class HashHelper : IHashHelper
{
    public byte[] CalculateMD5Hash(byte[] input)
    {
        using var md5 = MD5.Create();
        return md5.ComputeHash(input);
    }

    public byte[] CalculateMD5Hash(string input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        return CalculateMD5Hash(inputBytes);
    }

    private byte[] CalculateMd5Hash(FileStream stream)
    {
        using var md5 = MD5.Create();
        return md5.ComputeHash(stream);
    }

    public byte[] CalculateMD5HashFromFile(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return CalculateMd5Hash(stream);
    }

    public bool CompareByteArrays(byte[] arrayA, byte[] arrayB)
    {
        if (arrayA.Length != arrayB.Length) return false;

        for (int i = 0; i < arrayA.Length; i++)
        {
            if (arrayA[i] != arrayB[i]) return false;
        }

        return true;
    }
}