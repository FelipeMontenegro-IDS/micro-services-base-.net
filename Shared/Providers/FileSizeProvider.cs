using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;

namespace Shared.Providers;

public static class FileSizeProvider
{
    private static readonly Dictionary<FileSize, long> FileSizes = new()
    {
        { FileSize.Mb1, FileSizeConstants.Mb1 },
        { FileSize.Mb2, FileSizeConstants.Mb2 },
        { FileSize.Mb5, FileSizeConstants.Mb5 },
        { FileSize.Mb10, FileSizeConstants.Mb10 },
        { FileSize.Mb50, FileSizeConstants.Mb50 },
        { FileSize.Mb100, FileSizeConstants.Mb100 },
        { FileSize.Mb200, FileSizeConstants.Mb200 },
        { FileSize.Mb500, FileSizeConstants.Mb500 },
        { FileSize.Gb1 ,FileSizeConstants.Gb1},
        { FileSize.Gb2 ,FileSizeConstants.Gb2},
        { FileSize.Gb3 ,FileSizeConstants.Gb3}
    };

    public static long GetFileSize(FileSize fileSize)
    {
        if (FileSizes.TryGetValue(fileSize, out var getFileSize))
            return getFileSize;

        throw new ArgumentOutOfRangeException(nameof(fileSize), "tamaño no soportado.");
    }

    public static FileSize GetFileSize(long fileSize)
    {
        var mapping =
            FileSizes.FirstOrDefault(kv => kv.Value.Equals(fileSize));
        if (ValidationHelper.IsNotNull<FileSize>(mapping.Key) && ValidationHelper.IsNotNull<long>(mapping.Value))
            return mapping.Key;

        throw new ArgumentOutOfRangeException(nameof(fileSize), "tamaño no soportado.");
    }
    
    public static IEnumerable<long> GetAllFileSizes()
    {
        return FileSizes.Values;
    }
}