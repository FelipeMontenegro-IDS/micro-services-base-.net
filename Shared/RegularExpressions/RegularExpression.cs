using System.Text.RegularExpressions;

namespace Shared.RegularExpressions;

public static class RegularExpression
{
    public static readonly Regex NameQueue = new("^[a-z0-9._/-]{1,260}$", RegexOptions.IgnoreCase);
    public static readonly Regex AesKeyLength = new("^.{32}$");
    public static readonly Regex FormatEmail = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
}