
partial struct ReplaceOp {
    public string Regexp;
    public string Value;
    public string[] Destination;

    [GeneratedRegex(@"^\s*Replace\s*\(
    (?:\s*
    | ^\s*Regexp\s*=\s*(?<Regexp>.*)\s*$
    | ^\s*Destination\s*=\s*(?<Destination>.*)\s*$
    | ^\s*Value\s*=\s*(?<Value>.*)\s*$
    \s*)*\)", UIGenCfg.REGEXOPTS)]
    private static partial Regex Regex();

    public static ReplaceOp[] Parse(string source) => [.. Regex().Matches(source).Select(m => new ReplaceOp() {
        Regexp = m.Groups.GetValueOrDefault("Regexp")?.Value ?? string.Empty,
        Value = m.Groups.GetValueOrDefault("Value")?.Value ?? string.Empty,
        Destination = m.Groups.GetValueOrDefault("Destination")?.Value.Split(',', StringSplitOptions.TrimEntries) ?? []
    })];
}
