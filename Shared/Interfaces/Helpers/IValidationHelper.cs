using System.Diagnostics.CodeAnalysis;
using Shared.Enums;

namespace Shared.Interfaces.Helpers;

public interface IValidationHelper
{

    bool IsNull<T>(T? value);
    
    bool IsNull<T>(T? value) where T : struct;
    
    bool IsNotNull<T>([NotNullWhen(true)] T? value);
    
    bool IsNotNull<T>([NotNullWhen(true)] T? value) where T : struct;
    
    bool IsValidEnum<T>(T? value) where T : struct,Enum;

}