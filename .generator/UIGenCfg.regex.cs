partial struct UIGenCfg {
    public const RegexOptions REGEXOPTS = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline;

    [GeneratedRegex(@"\[[^\[\]]*(((?<Open>\[)[^\[\]]*)+((?<Close-Open>\])[^\[\]]*)+)*(?(Open)(?!))\]", REGEXOPTS)]
    private static partial Regex BracketRegex();

    [GeneratedRegex(@"\[\s*calc\s+(?<Calc>[^\[\]]*)\s*\]", RegexOptions.IgnoreCase)]
    private static partial Regex CalcRegex();

    [GeneratedRegex(@"\s*TemplateFilename\s*=\s*(?<TemplateFilename>.*)\s*$", REGEXOPTS)]
    private static partial Regex TemplateRegex();

    [GeneratedRegex(@"\s*OutputFormat\s*=\s*(?<OutputFormat>.*)\s*$", REGEXOPTS)]
    private static partial Regex OutputFormatRegex();

    [GeneratedRegex(@"\s*ForceGen(?:eration)?\s*=\s*(?<ForceGeneration>.*)\s*$", REGEXOPTS)]
    private static partial Regex ForceGenerationRegex();

    [GeneratedRegex(@"\s*StartIndex\s*=\s*(?<StartIndex>.*)\s*$", REGEXOPTS)]
    private static partial Regex StartIndexRegex();

    [GeneratedRegex(@"\s*Count\s*=\s*(?<Count>.*)\s*$", REGEXOPTS)]
    private static partial Regex CountRegex();

    [GeneratedRegex(@"^\s*Param\s*\(
    (?:\s*
    ^\s*(?<Key>.*)\s*=\s*(?<Value>.*)\s*$
    \s*)*\)", REGEXOPTS)]
    private static partial Regex ParamRegex();

    [GeneratedRegex(@"^\s*Global\s*\(
    (?:\s*
    ^\s*(?<Key>.*)\s*=\s*(?<Value>.*)\s*$
    \s*)*\)", REGEXOPTS)]
    public static partial Regex GlobalRegex();
}