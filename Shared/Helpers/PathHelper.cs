using Shared.Interfaces.Helpers;

namespace Shared.Helpers;

public class PathHelper : IPathHelper
{
    private readonly IValidationHelper _validationHelper;

    public PathHelper(IValidationHelper validationHelper)
    {
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
    }

    public string BuildFolderPath(IEnumerable<string>? subFolders)
    {
        return _validationHelper.IsNotNull(subFolders) &&
               _validationHelper.HasValues(subFolders)
            ? string.Join("/", subFolders)
            : string.Empty;
    }

    public string CombinePaths(string pathA, string pathB)
    {
        return Path.Combine(pathA, pathB);
    }

    public string GetFileNameWithoutExtension(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    public string GetFileExtension(string filePath)
    {
        return Path.GetExtension(filePath);
    }

    public bool IsValidPath(string path)
    {
        return !string.IsNullOrWhiteSpace(path) && Path.IsPathRooted(path);
    }

    public string BuildFullPath(string fileName, string folderPath)
    {
        return string.IsNullOrEmpty(folderPath) ? fileName : $"{folderPath}/{fileName}";
    }
}