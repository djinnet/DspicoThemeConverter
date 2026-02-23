namespace DspicoThemeForms.Core.Helper;

public class EnumWithName<T>
{
    public string? Name { get; set; }
    public T? Value { get; set; }

    public static EnumWithName<T>[] ParseEnum()
    {
        List<EnumWithName<T>> list = [];

        foreach (object o in Enum.GetValues(typeof(T)))
        {
            string? name = Enum.GetName(typeof(T), o);

            list.Add(new EnumWithName<T>
            {
                Name = name?.Replace('_', ' '),
                Value = (T)o
            });
        }

        return [.. list];
    }
}
