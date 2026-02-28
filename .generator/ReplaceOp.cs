
partial struct ReplaceOp {
    public string Regexp;
    public string Value;
    public string[] Target;

    [GeneratedRegex(@"^\s*Replace\s*\(
    (?:\s*
    | ^\s*Regexp\s*=\s*(?<Regexp>.*)\s*$
    | ^\s*Target\s*=\s*(?<Target>.*)\s*$
    | ^\s*Value\s*=\s*(?<Value>.*)\s*$
    \s*)*\)", UIGenCfg.REGEXOPTS)]
    private static partial Regex Regex();

    public static ReplaceOp[] Parse(string source) => [.. Regex().Matches(source).Select(m => new ReplaceOp() {
        Regexp = m.Groups.GetValueOrDefault("Regexp")?.Value ?? string.Empty,
        Value = m.Groups.GetValueOrDefault("Value")?.Value ?? string.Empty,
        Target = m.Groups.GetValueOrDefault("Target")?.Value.Split(',', StringSplitOptions.TrimEntries) ?? []
    })];
}
