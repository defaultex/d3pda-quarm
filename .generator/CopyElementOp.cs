struct BufferedCopyOp {
    public string Destination;
    public string[] Regexps;
    public string[] Outputs;
}

partial struct CopyElementOp {
    public string Type;
    public string Name;
    public string Destintion;
    public readonly string ElementRegexp { get => @$"<\s*{Type}\s*item\s*=\s*""{Name}""\s*>(?:.|\n)*?</{Type}\s*>"; }

    [GeneratedRegex(@"^\s*CopyElement\s*\(
    (?:\s*
    | ^\s*Element\s*=\s*(?<Type>.*)\s*:\s*(?<Name>.*)\s*$
    | ^\s*Destination\s*=\s*(?<Destination>.*)\s*$
    \s*)*\)", UIGenCfg.REGEXOPTS)]
    public static partial Regex Regex();

    public static CopyElementOp[] Parse(string source) => [.. Regex().Matches(source).Select(m => new CopyElementOp() {
        Type = m.Groups.GetValueOrDefault("Type")?.Value ?? string.Empty,
        Name = m.Groups.GetValueOrDefault("Name")?.Value ?? string.Empty,
        Destintion = m.Groups.GetValueOrDefault("Destination")?.Value ?? string.Empty
    })];
}