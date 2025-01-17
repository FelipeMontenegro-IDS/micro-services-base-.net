using Shared.Bases.Lookup;
using Shared.Constants.Data;
using Shared.Enums.Data;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Data;

namespace Shared.Providers.Data;

public class FileSizeProvider : BaseLookupProvider<FileSize, long>, IFileSizeProvider
{
    public FileSizeProvider(IValidationHelper validationHelper) : base(new Dictionary<FileSize, long>
    {
        { FileSize.Mb1, FileSizeConstant.Mb1 },
        { FileSize.Mb2, FileSizeConstant.Mb2 },
        { FileSize.Mb5, FileSizeConstant.Mb5 },
        { FileSize.Mb10, FileSizeConstant.Mb10 },
        { FileSize.Mb50, FileSizeConstant.Mb50 },
        { FileSize.Mb100, FileSizeConstant.Mb100 },
        { FileSize.Mb200, FileSizeConstant.Mb200 },
        { FileSize.Mb500, FileSizeConstant.Mb500 },
        { FileSize.Gb1, FileSizeConstant.Gb1 },
        { FileSize.Gb2, FileSizeConstant.Gb2 },
        { FileSize.Gb3, FileSizeConstant.Gb3 }
    }, validationHelper)
    {
    }
}