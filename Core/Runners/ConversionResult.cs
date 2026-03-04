namespace DspicoThemeForms.Core.Runners;

/// <summary>
/// Represents the outcome of a conversion operation, including its success status, any errors encountered, and the
/// execution results of related commands.
/// </summary>
/// <remarks>Use the ConversionResult to determine whether a conversion was successful, inspect any errors that
/// occurred during the process, and review the execution status of individual commands associated with the conversion.
/// This type is typically returned by conversion routines to provide comprehensive feedback about the
/// operation.</remarks>
public record ConversionResult
{
    public bool Success { get; set; } = false;
    public IList<string> Errors { get; set; } = Array.Empty<string>();
    public IDictionary<string, bool> Commands { get; set; } = new Dictionary<string, bool>();

    /// <summary>
    /// Gets a value indicating whether any validation errors are present.
    /// </summary>
    /// <remarks>Use this property to determine if the object is in a valid state before performing operations
    /// that require all validations to pass.</remarks>
    public bool HasErrors => Errors.Count > 0;

    /// <summary>
    /// Determines whether all commands in the collection have been executed successfully.
    /// </summary>
    /// <remarks>Use this method to verify the overall success of all commands after execution. It is useful
    /// for batch operations where the aggregate result is important.</remarks>
    /// <returns>Returns <see langword="true"/> if every command in the collection was executed successfully; otherwise, <see
    /// langword="false"/>.</returns>
    public bool DidAllCommandRan() => Commands.Values.All(success => success);

    /// <summary>
    /// Determines whether at least one command in the collection has executed successfully.
    /// </summary>
    /// <remarks>This method checks the execution status of all commands in the collection. It is useful for
    /// verifying if any command has completed without failure.</remarks>
    /// <returns>Returns <see langword="true"/> if any command has run successfully; otherwise, <see langword="false"/>.</returns>
    public bool DidAnyCommandRan() => Commands.Values.Any(success => success);

    /// <summary>
    /// Determines whether none of the commands in the collection have been executed successfully.
    /// </summary>
    /// <remarks>Use this method to check the overall execution state of commands before proceeding with
    /// further logic. It evaluates the success status of all commands stored in the collection.</remarks>
    /// <returns>Returns <see langword="true"/> if all commands have not run successfully; otherwise, <see langword="false"/>.</returns>
    public bool DidNoneCommandRan() => Commands.Values.All(success => !success);
}
