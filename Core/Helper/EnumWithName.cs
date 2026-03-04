namespace DspicoThemeForms.Core.Helper;

/// <summary>
/// Represents a member of an enumeration, encapsulating its name and value.
/// </summary>
/// <remarks>This class is useful for scenarios where enumeration members need to be displayed in a user-friendly
/// format, such as in user interfaces or during serialization.</remarks>
/// <typeparam name="T">The type of the enumeration member.</typeparam>
public class EnumWithName<T>
{
    public string? Name { get; set; }
    public T? Value { get; set; }

    /// <summary>
    /// Parses all members of the enumeration type T and returns an array containing their names and values.
    /// </summary>
    /// <remarks>The returned names have underscores replaced with spaces to improve readability. This method
    /// is useful for displaying enumeration values in user interfaces or for serialization purposes.</remarks>
    /// <returns>An array of EnumWithName<T> objects, each representing a member of the enumeration T with its name and value.</returns>
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
