using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;

namespace Shared.Providers;

public static class FileSizeProvider
{
    private static readonly Dictionary<FileSize, long> FileSizes = new()
    {
        { FileSize.MB1, FileSizeConstants.MB1 },
        { FileSize.MB2, FileSizeConstants.MB2 },
        { FileSize.MB5, FileSizeConstants.MB5 },
        { FileSize.MB10, FileSizeConstants.MB10 },
        { FileSize.MB50, FileSizeConstants.MB50 },
        { FileSize.MB100, FileSizeConstants.MB100 },
        { FileSize.MB200, FileSizeConstants.MB200 },
        { FileSize.MB500, FileSizeConstants.MB500 }
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
}