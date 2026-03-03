using DspicoThemeForms.Core.Enums;

namespace DspicoThemeForms.Core.Helper;

public static class EnumHelper
{
    public static EnumWithName<T>[] ConditionEnum<T>(this EnumWithName<T>[] list, Func<EnumWithName<T>, bool> condition)
    {
        List<EnumWithName<T>> result = [];
        foreach (EnumWithName<T> item in list)
        {
            if (condition(item))
            {
                result.Add(item);
            }
        }
        return [.. result];
    }

    public static bool FileChecking(this EgatesFormat format, params string?[] inputs)
    {
        bool result = false;
        switch (format)
        {
            case EgatesFormat.NONE:
                result = true;
                break;
            case EgatesFormat.AND:
                result = inputs.All(input => !string.IsNullOrEmpty(input));
                break;
            case EgatesFormat.OR:
                result = inputs.Any(input => !string.IsNullOrEmpty(input));
                break;
            case EgatesFormat.XOR:
                int count = inputs.Count(input => !string.IsNullOrEmpty(input));
                result = count == 1;
                break;
        }
        return result;
    }
}
