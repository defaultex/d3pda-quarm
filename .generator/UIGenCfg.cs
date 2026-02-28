/* '.uigencfg' File Formats
 *
 *  TemplateFilename = somefile.xmltemplate
 *  OutputFormat = [Name].xml
 *  ForceGeneration = false     # forces the generator to run even if not needed
 *  StartIndex = 0
 *  Count = 4
 *
 *  # Define global parameters to provide to all files.
 *  Global(
 *      [Key] = [Value]
 *      ...
 *  )
 *
 *  # Define enumerated parameters to provide to each generated file.
 *  Param(
 *      [Key] = [Value0, Value1, ...]
 *      ...
 *  )
 *
 *  # Replace text using regex in the specified file
 *  Replace(
 *      Regexp = <FillTint>\s*<R>240</R>\s*<G>0</G>\s*<B>0</B>\s*</FillTint>
 *      Target = player_window/MP.xml
 *      Value = <FillTint><R>0</R><G>0</G><B>240</B></FillTint>
 *  )
 *
 *  # Copy an element from the generated file into the specified target file.
 *  CopyElement(
 *      Element = [Type]:[Name] (ex. Screen:HPLabel_Layout)
 *      Destination = Filename (ex. EQUI_PlayerWindow.xml)
 *  )
 *
 * + [EQLabelType.Mana] notation can be used to fetch parameters built-in and custom.
 * + [calc i * 6 + Math.Floor(4.5)] notation can be used to evaluate math in the parser.
 * |- Parameters can be accessed in calculation without bracket notation.
 * |- The parses used a DotNetContext meaning it has access to .Net's math functions.
 *
 */

using System.Diagnostics;
using MathEvaluation.Context;
using MathEvaluation.Extensions;

partial struct UIGenCfg {
    static readonly Dictionary<string, string> ms_systemParams = new(Enumerable.Concat(
        Enum.GetNames<EQLabelType>().Select(name => new KeyValuePair<string, string>($"EQLabelType.{name}",  $"{(int)Enum.Parse<EQLabelType>(name)}")),
        Enum.GetNames<EQGaugeType>().Select(name => new KeyValuePair<string, string>($"EQGaugeType.{name}",  $"{(int)Enum.Parse<EQGaugeType>(name)}"))
    ));

    readonly Dictionary<string, string> m_parameters;

    public readonly string GenCfgPath;
    public readonly string TemplateFilename;
    public readonly string OutputFormat;
    public readonly bool ForceGeneration;
    public readonly int StartIndex;
    public readonly int Count;
    public readonly ReplaceOp[] ReplaceOps;
    public readonly CopyElementOp[] CopyElementOps;
    public readonly Dictionary<string, string[]> Params;
    public readonly Dictionary<string, string> Globals;

    public UIGenCfg(string gencfgPath) {
        GenCfgPath = gencfgPath;
        string source = File.ReadAllText(GenCfgPath);

        TemplateFilename = TemplateRegex().Match(source).Groups.GetValueOrDefault("TemplateFilename")?.Value ?? string.Empty;
        OutputFormat = OutputFormatRegex().Match(source).Groups.GetValueOrDefault("OutputFormat")?.Value ?? string.Empty;
        _ = bool.TryParse(ForceGenerationRegex().Match(source).Groups.GetValueOrDefault("ForceGeneration")?.Value ?? bool.FalseString, out ForceGeneration);
        _ = int.TryParse(StartIndexRegex().Match(source).Groups.GetValueOrDefault("StartIndex")?.Value ?? "0", out StartIndex);
        _ = int.TryParse(CountRegex().Match(source).Groups.GetValueOrDefault("Count")?.Value ?? "0", out Count);

        m_parameters = new(ms_systemParams);

        Globals = [];
        foreach (Match m in GlobalRegex().Matches(source)) {
            CaptureCollection? keys = m.Groups.GetValueOrDefault("Key")?.Captures,
                               values = m.Groups.GetValueOrDefault("Value")?.Captures;
            for (int i = 0; i < (keys?.Count ?? 0); i++) {
                if (!string.IsNullOrWhiteSpace(keys[i].Value)) {
                    string key = keys[i].Value.Trim();
                    string value = values[i].Value.Trim();
                    Globals[key] = value;
                    m_parameters[key] = value;
                }
            }
        }

        Params = [];
        foreach (Match m in ParamRegex().Matches(source)) {
            CaptureCollection? keys = m.Groups.GetValueOrDefault("Key")?.Captures,
                               values = m.Groups.GetValueOrDefault("Value")?.Captures;
            for (int i = 0; i < (keys?.Count ?? 0); i++) {
                if (!string.IsNullOrWhiteSpace(keys[i].Value)) {
                    string key = keys[i].Value.Trim();
                    string value = values[i].Value.Trim();
                    Params[key] = value.Split(',', StringSplitOptions.TrimEntries) ?? [];
                }
            }
        }

        ReplaceOps = ReplaceOp.Parse(source);
        CopyElementOps = CopyElementOp.Parse(source);
    }

    public string ReplaceParameters(string source, bool verbose = false) {
        string result = source;

        MatchCollection matches = BracketRegex().Matches(source);
        foreach (Match bracketMatch in matches) {
            if (!bracketMatch.Success) { continue; }

            string key = bracketMatch.Value.TrimStart('[').TrimEnd(']');
            if (key != null && m_parameters.TryGetValue(key, out string value)) {
                result = result.Replace(bracketMatch.Value, value);
                Debug.WriteLineIf(verbose, $"   parameter: [{key}] => {value}");
            }
        }

        foreach (string key in m_parameters.Keys) {
            string value = m_parameters[key];
            foreach (Match m in Regex.Matches(result, @$"\[\s*(?<Param>{key})\s*\]")) {
                result = result.Replace(m.Value, value);
                Debug.WriteLineIf(verbose, $"   parameter: [{key}] => {value}");
            }
        }

        return result;
    }

    public string ProcessString(string source, bool verbose = false) {
        string result = source;
        result = ReplaceParameters(result, verbose);
        foreach (Match m in CalcRegex().Matches(result)) {
            if (m.Groups.TryGetValue("Calc", out Group g)) {
                string value = $"{g.Value.Evaluate(m_parameters, new DotNetStandardMathContext())}";
                result = result.Replace(m.Captures[0].Value, value);
                Debug.WriteLineIf(verbose, $"   calc: [{g.Value}] => {value}");
            }
        }
        result = ReplaceParameters(result, verbose);
        return result;
    }

    public bool ShouldGen(string outputFile) =>
        !File.Exists(outputFile) ||
        File.GetLastWriteTime(TemplateFilename) > File.GetLastWriteTime(outputFile) ||
        File.GetLastWriteTime(GenCfgPath) > File.GetLastWriteTime(outputFile);

    public bool GenerateFiles() {
        bool result = false;

        // batch changes to give any previous writes time to finish
        BufferedCopyOp[] bco = new BufferedCopyOp[CopyElementOps?.Length ?? 0];
        for (int i = 0; i < CopyElementOps.Length; i++) {
            bco[i] = new BufferedCopyOp() {
                Destination = CopyElementOps[i].Destintion,
                Regexps = new string[Count],
                Outputs = new string[Count]
            };
            for (int j = StartIndex; j < Count; j++) {
                bco[i].Regexps[j] = CopyElementOps[i].ElementRegexp;
            }
        }

        List<string> tempParams = [];
        for (int i = StartIndex; i < Count; i++) {
            // add local parameters
            tempParams.Add("i");
            m_parameters["i"] = $"{i}";
            foreach (KeyValuePair<string, string[]> kvp in Params) {
                m_parameters[kvp.Key] = kvp.Value[i - StartIndex];
                tempParams.Add(kvp.Key);
            }

            // perform replacement operations
            string output = null, filename = ProcessString(OutputFormat);
            if (ForceGeneration || ShouldGen(filename)) {
                Console.WriteLine($"   Generating {filename}");
                output = File.ReadAllText(TemplateFilename);
                output = ProcessString(output, true);
                foreach (ReplaceOp replaceOp in ReplaceOps) {
                    if (replaceOp.Target.Contains(filename)) {
                        output = Regex.Replace(output, replaceOp.Regexp, replaceOp.Value);
                        Console.WriteLine($"   replace ({filename}): {replaceOp.Regexp} => {replaceOp.Value}");
                    }
                }
                File.WriteAllText(filename, output);
            }
            // else { Console.WriteLine($"   Skipping {filename}, file is newer than the template."); }
            result |= output != null;

            // batch copy operations
            for (int j = 0; j < bco.Length; j++) {
                bco[j].Regexps[i] = ProcessString(bco[j].Regexps[i]);
                bco[j].Outputs[i] = (output != null) ? Regex.Match(output, bco[j].Regexps[i])?.Value : null;
            }
        }
        foreach (string key in tempParams) {
            m_parameters.Remove(key);
        }

        // perform the batched copy operations
        foreach (BufferedCopyOp copyOp in bco) {
            string targetXml = File.ReadAllText(copyOp.Destination);
            bool doWrite = false;
            for (int i = 0; i < copyOp.Regexps.Length; i++) {
                if (copyOp.Outputs[i] != null) {
                    targetXml = Regex.Replace(targetXml, copyOp.Regexps[i], copyOp.Outputs[i]);
                    Console.WriteLine($"   copy ({copyOp.Destination}): {copyOp.Regexps[i]} => {copyOp.Outputs[i]?.Split('\n')[0]}");
                    doWrite = true;
                }
            }
            if (doWrite) { File.WriteAllText(copyOp.Destination, targetXml); }
        }
        return result;
    }

    public readonly void WriteConsole() {
        Console.WriteLine(GenCfgPath);
        Console.WriteLine($"   Template: {TemplateFilename}");
        Console.WriteLine($"   OutputFormat: {OutputFormat}");
        Console.WriteLine($"   ForceGen: {ForceGeneration}");
        Console.WriteLine($"   StartIndex: {StartIndex}");
        Console.WriteLine($"   Count: {Count}");
        if (Params.Count > 0) {
            Console.WriteLine("   Params");
            _ = Params.All(p => {
                Console.WriteLine($"      {p.Key}: {string.Join(", ", p.Value)}");
                return true;
            });
        }
        _ = ReplaceOps.All(r => {
            Console.WriteLine("   Replace");
            Console.WriteLine($"      Regexp: {r.Regexp}");
            Console.WriteLine($"      Value: {r.Value}");
            Console.WriteLine($"      Targets: {string.Join(", ", r.Target)}");
            return true;
        });
        // _ = CopyOps.All(c => {
        //     Console.WriteLine("   Copy");
        //     Console.WriteLine($"      Regexp: {c.Regexp}");
        //     Console.WriteLine($"      Target: {c.Target}");
        //     return true;
        // });
    }
}