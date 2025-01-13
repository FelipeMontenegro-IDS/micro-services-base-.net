using Shared.Enums;
using Shared.Interfaces.LookupProvider;

namespace Shared.Interfaces.Providers;

public interface IDateFormatProvider :  ILookupProvider<DateFormat, string>
{
}