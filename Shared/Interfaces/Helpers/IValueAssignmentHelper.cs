namespace Shared.Interfaces.Helpers;

public interface IValueAssignmentHelper
{
    public void SetIfNotNullOrEmpty(Action<string> assignAction, string? value);
    public void SetIfNotNull<T>(Action<T> assignAction, T? value);
    public void SetIfNotNull<T>(Action<T?> assignAction, T? value) where T : struct;
    public void SetIf<T>(Action<T> assignAction, T? value, Func<T, bool> predicate);
    public void SetIf<T>(Action<T> assignAction, T? value, Func<T, bool> predicate) where T : struct;
    public void SetToStringIfNotNull<T>(Action<string> assignAction, T? value) where T : class;
    public void SetDefaultIfNull<T>(Action<T> assignAction, T? value, T defaultValue);
}