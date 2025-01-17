using Shared.Enums.Time;

namespace Shared.Interfaces.Providers.Time;

public interface IDateFormatProvider : Lookup.ILookupProvider<DateFormat, string>
{
}