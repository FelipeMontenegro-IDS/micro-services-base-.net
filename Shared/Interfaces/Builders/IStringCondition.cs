namespace Shared.Interfaces.Builders;

public interface IStringCondition<out TBuilder, T>
{
    TBuilder MinLength(Func<T, string?> selector, int minLength);

    TBuilder MaxLength(Func<T, string?> selector, int maxLength);
    
    TBuilder RegexMatch(Func<T, string?> selector, string pattern);
    
    TBuilder Empty(Func<T, string?> selector);

    TBuilder NotEmpty(Func<T, string?> selector);
    
    TBuilder Email(Func<T, string?> selector);
    
    TBuilder EndsWith(Func<T, string> selector, string value);
 
    TBuilder StartsWith(Func<T, string> selector, string value);
    
    TBuilder Contains(Func<T, string> selector, string value);

}