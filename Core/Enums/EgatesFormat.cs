namespace DspicoThemeForms.Core.Enums;

/// <summary>
/// Specifies the logical gate operation formats available for combining multiple conditions.
/// </summary>
/// <remarks>Use this enumeration to indicate how logical conditions should be evaluated in operations. The
/// available formats include AND, OR, and XOR, allowing for flexible control over condition aggregation. This type is
/// commonly used in scenarios where multiple logical rules must be applied to determine an outcome.</remarks>
public enum EgatesFormat
{
    NONE = 0,
    AND = 1,
    OR = 2,
    XOR = 3,
}
