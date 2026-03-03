namespace DspicoThemeForms.Core.Runners;

public record ConversionResult
{
    public bool Success { get; set; } = false;
    public IList<string> Errors { get; set; } = Array.Empty<string>();
    public IDictionary<string, bool> Commands { get; set; } = new Dictionary<string, bool>();

    public bool HasErrors => Errors.Count > 0;

    public bool DidAllCommandRan() => Commands.Values.All(success => success);

    public bool DidAnyCommandRan() => Commands.Values.Any(success => success);

    public bool DidNoneCommandRan() => Commands.Values.All(success => !success);
}
