/* '.uigencfg' File Formats
 *
 *  TemplateFilename = somefile.xmltemplate
 *  OutputFormat = outfile[i].xml
 *  ForceGeneration = false     # forces the generator to run even if not needed

 * Iterative:
 *  StartIndex = 0
 *  Count = 4
 *
 * Enumerative:
 *  TemplateFilename = somefile.xmltemplate
 *  OutputFormat = [Enum].xml
 *  ForceGeneration = false
 *  Enum = A, B, C
 *
 *  + Define custom parameters to provide to each generated file.
 *  Param(
 *      Key = Name
 *      Value = HP, Mana, Fatigue
 *  )
 *
 *  + Replace text using regex in the specified file
 *  Replace(
 *      Regexp = <FillTint>\s*<R>240</R>\s*<G>0</G>\s*<B>0</B>\s*</FillTint>
 *      Target = player_window/MP.xml
 *      Value = <FillTint><R>0</R><G>0</G><B>240</B></FillTint>
 *  )
 *
 *  + Copy from the generated file using regex into the specified target file.
 *  CopyOp(
 *      Regexp = <Screen item="SW_[Enum]Label_Layout">(?:.|\n)*?</Screen>
 *      Target = EQUI_PlayerWindow.xml
 *  )
 *
 * + [EQLabelType.Mana] notation can be used to fetch parameters built-in and custom.
 * + [calc i * 6 + Math.Floor(4.5)] notation can be used to evaluate math in the parser.
 * |- Parameters can be accessed in calculation without bracket notation.
 * |- The parses used a DotNetContext meaning it has access to .Net's math functions.
 *
 */

partial struct UIGenCfg {
    #region Regex

    const RegexOptions REGEXOPTS = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline;

    [GeneratedRegex(@"\s*TemplateFilename\s*=\s*(?<TemplateFilename>.*)\s*$", REGEXOPTS)]
    private static partial Regex TemplateRegex();

    [GeneratedRegex(@"\s*OutputFormat\s*=\s*(?<OutputFormat>.*)\s*$", REGEXOPTS)]
    private static partial Regex OutputFormatRegex();

    [GeneratedRegex(@"\s*ForceGeneration\s*=\s*(?<ForceGeneration>.*)\s*$", REGEXOPTS)]
    private static partial Regex ForceGenerationRegex();

    [GeneratedRegex(@"\s*Enum\s*=\s*(?<Enum>.*)\s*$", REGEXOPTS)]
    private static partial Regex EnumRegex();

    [GeneratedRegex(@"\s*StartIndex\s*=\s*(?<StartIndex>.*)\s*$", REGEXOPTS)]
    private static partial Regex StartIndexRegex();

    [GeneratedRegex(@"\s*Count\s*=\s*(?<Count>.*)\s*$", REGEXOPTS)]
    private static partial Regex CountRegex();

    [GeneratedRegex(@"^\s*Copy\s*\(
    (?:\s*\n
    | ^\s*Regexp\s*=\s*(?<Regexp>.*)\s*$
    | ^\s*Target\s*=\s*(?<Target>.*)\s*$
    )*\)", REGEXOPTS)]
    private static partial Regex CopyRegex();

    [GeneratedRegex(@"^\s*Param\s*\(
    (?:\s*\n
    | ^\s*Key\s*=\s*(?<Key>.*)\s*$
    | ^\s*Value\s*=\s*(?<Value>.*)\s*$
    )*\)", REGEXOPTS)]
    private static partial Regex ParamRegex();

    [GeneratedRegex(@"^\s*Replace\s*\(
    (?:\s*\n
    | ^\s*Regexp\s*=\s*(?<Regexp>.*)\s*$
    | ^\s*Target\s*=\s*(?<Target>.*)\s*$
    | ^\s*Value\s*=\s*(?<Value>.*)\s*$
    )*\)", REGEXOPTS)]
    private static partial Regex ReplaceRegex();

    #endregion

    public readonly string GenCfgPath;
    public string TemplateFilename;
    public string OutputFormat;
    public bool ForceGeneration;
    public string[] Enum;
    public int StartIndex;
    public int Count;
    public CopyOperation[] CopyOps;
    public ReplaceOperation[] ReplaceOps;
    public Dictionary<string, string[]> Parameters;

    public bool IsEnumerative { get => (Enum?.Length ?? 0) > 0; }
    public bool IsIterative { get => (Enum?.Length ?? 0) < 1; }

    public UIGenCfg(string gencfgPath) {
        GenCfgPath = gencfgPath;
        string dir = Path.GetDirectoryName(GenCfgPath);
        string src = File.ReadAllText(GenCfgPath);
        Group g;

        if (TemplateRegex().Match(src).Groups.TryGetValue("TemplateFilename", out g)) {
            TemplateFilename = Path.Combine(dir, g.Value).Replace("./", string.Empty);
        }
        if (OutputFormatRegex().Match(src).Groups.TryGetValue("OutputFormat", out g)) {
            OutputFormat = Path.Combine(dir, g.Value).Replace("./", string.Empty);
        }
        _ = ForceGenerationRegex().Match(src).Groups.TryGetValue("ForceGeneration", out g) &&
            bool.TryParse(g.Value.Trim(), out ForceGeneration);

        if (EnumRegex().Match(src).Groups.TryGetValue("Enum", out g)) {
            Enum = !string.IsNullOrWhiteSpace(g.Value) ? g.Value.Split(',', StringSplitOptions.TrimEntries) : [];
            StartIndex = 0;
            Count = Enum.Length;
        } else {
            _ = StartIndexRegex().Match(src).Groups.TryGetValue("StartIndex", out g) &&
                int.TryParse(g.Value, out StartIndex);
            _ = CountRegex().Match(src).Groups.TryGetValue("Count", out g) &&
                int.TryParse(g.Value, out Count);
        }

        Parameters = [];
        foreach (Match m in ParamRegex().Matches(src)) {
            CaptureCollection keys = m.Groups.GetValueOrDefault("Key")?.Captures;
            CaptureCollection values = m.Groups.GetValueOrDefault("Value")?.Captures;
            for (int i = 0; i < (keys?.Count ?? 0); i++) {
                if (!string.IsNullOrWhiteSpace(keys[i].Value)) {
                    Parameters[keys[i].Value] = values[i].Value?.Split(',', StringSplitOptions.TrimEntries) ?? [];
                }
            }
        }

        List<CopyOperation> copyOps = [];
        foreach (Match m in CopyRegex().Matches(src)) {
            CaptureCollection regexps = m.Groups.GetValueOrDefault("Regexp")?.Captures;
            CaptureCollection targets = m.Groups.GetValueOrDefault("Target")?.Captures;
            int count = regexps?.Count ?? 0;
            for (int i = 0; i < count; i++) {
                copyOps.Add(new() {
                    Regexp = regexps[i].Value ?? string.Empty,
                    Target = targets[i].Value ?? string.Empty
                });
            }
        }
        CopyOps = copyOps.ToArray();

        List<ReplaceOperation> replaceOps = [];
        foreach (Match mrepl in ReplaceRegex().Matches(src)) {
            CaptureCollection regexps = mrepl.Groups.GetValueOrDefault("Regexp")?.Captures;
            CaptureCollection values = mrepl.Groups.GetValueOrDefault("Value")?.Captures;
            CaptureCollection targets = mrepl.Groups.GetValueOrDefault("Target")?.Captures;
            int count = regexps?.Count ?? 0;
            for (int i = 0; i < count; i++) {
                replaceOps.Add(new() {
                    Regexp = regexps[i].Value ?? string.Empty,
                    Value = values[i].Value ?? string.Empty,
                    Target = i < targets.Count ?
                        targets[i].Value.Split(',', StringSplitOptions.TrimEntries) ?? [] : []
                });
            }
        }
        ReplaceOps = replaceOps.ToArray();
    }

    public void WriteConsole() {
        Console.WriteLine(GenCfgPath);
        Console.WriteLine($"   Template: {TemplateFilename}");
        Console.WriteLine($"   OutputFormat: {OutputFormat}");
        Console.WriteLine($"   ForceGen: {ForceGeneration}");
        if (IsEnumerative) {
            Console.WriteLine($"   Enum: {string.Join(", ", Enum)}");
        }
        if (IsIterative) {
            Console.WriteLine($"   StartIndex: {StartIndex}");
            Console.WriteLine($"   Count: {Count}");
        }
        if (Parameters.Count > 0) {
            Console.WriteLine("   Params");
            Parameters.All(p => {
                Console.WriteLine($"      {p.Key}: {string.Join(", ", p.Value)}");
                return true;
            });
        }
        ReplaceOps.All(r => {
            Console.WriteLine("   Replace");
            Console.WriteLine($"      Regexp: {r.Regexp}");
            Console.WriteLine($"      Value: {r.Value}");
            Console.WriteLine($"      Targets: {string.Join(", ", r.Target)}");
            return true;
        });
        CopyOps.All(c => {
            Console.WriteLine("   Copy");
            Console.WriteLine($"      Regexp: {c.Regexp}");
            Console.WriteLine($"      Target: {c.Target}");
            return true;
        });
    }
}