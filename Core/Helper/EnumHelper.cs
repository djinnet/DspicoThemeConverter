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
        return result.ToArray();
    }
}
