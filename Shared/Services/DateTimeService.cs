using Application.Interfaces;

namespace Shared.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime nowUtc { get; } = DateTime.UtcNow;
}