namespace Shared.Interfaces.Helpers;

public interface IPathHelper
{
    string BuildFolderPath(IEnumerable<string>? subFolders);
    string CombinePaths(string pathA, string pathB);
    string GetFileNameWithoutExtension(string filePath);
    string GetFileExtension(string filePath);
    bool IsValidPath(string path);
    string BuildFullPath(string fileName, string folderPath);

}