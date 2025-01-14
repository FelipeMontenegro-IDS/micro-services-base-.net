namespace Shared.Interfaces.Helpers;

public interface IHashHelper
{
    byte[] CalculateMD5Hash(byte[] input);
    byte[] CalculateMD5Hash(string input);
    byte[] CalculateMD5HashFromFile(string filePath);
    bool CompareByteArrays(byte[] arrayA, byte[] arrayB);
}