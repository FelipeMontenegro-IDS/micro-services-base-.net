using System.Linq.Expressions;

namespace Shared.Interfaces.Builders;

public interface IStringCondition<out TBuilder, T>
{
    TBuilder MinLength(Expression<Func<T, string?>> selector, int minLength);

    TBuilder MaxLength(Expression<Func<T, string?>> selector, int maxLength);

    TBuilder RegexMatch(Expression<Func<T, string?>> selector, string pattern);

    TBuilder Empty(Expression<Func<T, string?>> selector);

    TBuilder NotEmpty(Expression<Func<T, string?>> selector);

    TBuilder Email(Expression<Func<T, string?>> selector);

    TBuilder EndsWith(Expression<Func<T, string>> selector, string value);

    TBuilder StartsWith(Expression<Func<T, string>> selector, string value);

    TBuilder Contains(Expression<Func<T, string>> selector, string value);
}