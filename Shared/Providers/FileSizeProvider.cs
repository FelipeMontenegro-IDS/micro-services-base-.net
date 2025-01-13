using Shared.Bases.LookupProvider;
using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

public class FileSizeProvider : BaseLookupProvider<FileSize, long>, IFileSizeProvider
{
    public FileSizeProvider(IValidationHelper validationHelper) : base(new Dictionary<FileSize, long>
    {
        { FileSize.Mb1, FileSizeConstants.Mb1 },
        { FileSize.Mb2, FileSizeConstants.Mb2 },
        { FileSize.Mb5, FileSizeConstants.Mb5 },
        { FileSize.Mb10, FileSizeConstants.Mb10 },
        { FileSize.Mb50, FileSizeConstants.Mb50 },
        { FileSize.Mb100, FileSizeConstants.Mb100 },
        { FileSize.Mb200, FileSizeConstants.Mb200 },
        { FileSize.Mb500, FileSizeConstants.Mb500 },
        { FileSize.Gb1, FileSizeConstants.Gb1 },
        { FileSize.Gb2, FileSizeConstants.Gb2 },
        { FileSize.Gb3, FileSizeConstants.Gb3 }
    },validationHelper)
    {
    }
}