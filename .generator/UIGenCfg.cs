partial struct UIGenCfg {
    [GeneratedRegex(@"(?:\s*TemplateFilename\s*=\s*""(?<Template>(?:[^/]*/)*(?:.*))"")")]
    private static partial Regex TemplateRegex();

    [GeneratedRegex(@"(?:\s*OutputFormat\s*=\s*""(?<OutputFormat>(?:[^/]*/)*(?:.*))"")")]
    private static partial Regex OutputFormatRegex();

    [GeneratedRegex(@"(?:\s*StartIndex\s*=\s*(?<StartIndex>\d+))")]
    private static partial Regex StartIndexRegex();

    [GeneratedRegex(@"Count\s*=\s*(?<Count>\d+)")]
    private static partial Regex CountRegex();

    public string GenCfgPath;
    public string TemplateFilename;
    public string OutputFormat;
    public int StartIndex;
    public int Count;

    public UIGenCfg(string gencfgPath) {
        GenCfgPath = gencfgPath;
        Group g;
        string dir = Path.GetDirectoryName(GenCfgPath), source = File.ReadAllText(GenCfgPath);
        if (TemplateRegex().Match(source).Groups.TryGetValue("Template", out g)) {
            TemplateFilename = Path.Combine(dir, g.Value);
        }
        if (OutputFormatRegex().Match(source).Groups.TryGetValue("OutputFormat", out g)) {
            OutputFormat = Path.Combine(dir, g.Value);
        }
        _ = StartIndexRegex().Match(source).Groups.TryGetValue("StartIndex", out g) && int.TryParse(g.Value, out StartIndex);
        _ = CountRegex().Match(source).Groups.TryGetValue("Count", out g) && int.TryParse(g.Value, out Count);
    }

    public void WriteConsole() {
        Console.WriteLine(GenCfgPath);
        Console.WriteLine($"   Template: {TemplateFilename}");
        Console.WriteLine($"   OutputFormat: {OutputFormat}");
        Console.WriteLine($"   StartIndex: {StartIndex}");
        Console.WriteLine($"   Count: {Count}");
    }
}