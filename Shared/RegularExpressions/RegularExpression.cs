using System.Text.RegularExpressions;

namespace Shared.RegularExpressions;

public static class RegularExpression
{
    public static readonly Regex NameQueue = new("^[a-z0-9._/-]{1,260}$", RegexOptions.IgnoreCase);
}