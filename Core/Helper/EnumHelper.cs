using DspicoThemeForms.Core.Enums;

namespace DspicoThemeForms.Core.Helper;
/// <summary>
/// Provides static utility methods for working with enumeration types, including filtering enumeration values and
/// evaluating input strings based on specified formats.
/// </summary>
/// <remarks>The EnumHelper class offers methods that facilitate dynamic selection and evaluation of enumeration
/// values and related input data. These methods are designed to simplify common operations involving enumerations, such
/// as filtering based on custom criteria and validating input according to configurable formats. All methods are static
/// and do not modify the original data structures.</remarks>
public static class EnumHelper
{
    /// <summary>
    /// Filters an array of EnumWithName<T> elements based on a specified predicate.
    /// </summary>
    /// <remarks>Use this method to dynamically select enumeration values based on custom criteria. The
    /// original array is not modified.</remarks>
    /// <typeparam name="T">The enumeration type represented by each EnumWithName<T> element.</typeparam>
    /// <param name="list">An array of EnumWithName<T> instances to be filtered.</param>
    /// <param name="condition">A predicate function that determines whether an element should be included in the result. The function receives
    /// an EnumWithName<T> and returns <see langword="true"/> to include the element; otherwise, <see
    /// langword="false"/>.</param>
    /// <returns>An array of EnumWithName<T> containing the elements that satisfy the specified condition. If no elements match,
    /// an empty array is returned.</returns>
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

    /// <summary>
    /// Evaluates a set of input strings according to the specified format and returns a value indicating whether the
    /// evaluation is successful.
    /// </summary>
    /// <remarks>The method supports four evaluation formats: NONE, AND, OR, and XOR. When using AND, all
    /// input strings must be non-null and non-empty. OR returns true if at least one input string is non-null and
    /// non-empty. XOR returns true only if exactly one input string is non-null and non-empty. NONE always returns true
    /// regardless of the inputs.</remarks>
    /// <param name="format">The format used to evaluate the input strings. Supported values are NONE, AND, OR, and XOR.</param>
    /// <param name="inputs">An array of strings to be evaluated. Each string is checked for null or empty values based on the specified
    /// format.</param>
    /// <returns>true if the evaluation succeeds according to the specified format; otherwise, false.</returns>
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
