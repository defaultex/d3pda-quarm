using MathEvaluation.Context;
using MathEvaluation.Extensions;

partial class Program {
    #region Regex

    [GeneratedRegex(@"\[\s*EQLabelType.(?<LabelType>.*)\s*\]")]
    private static partial Regex EQLabelRegex();

    [GeneratedRegex(@"\[\s*EQGaugeType.(?<GaugeType>.*)\s*\]")]
    private static partial Regex EQGaugeRegex();

    [GeneratedRegex(@"\[\s*(?<Iter>i)\s*\]", RegexOptions.IgnoreCase)]
    private static partial Regex IterRegex();

    [GeneratedRegex(@"\[\s*(?<Enum>enum)\s*\]", RegexOptions.IgnoreCase)]
    private static partial Regex EnumRegex();

    [GeneratedRegex(@"\[\s*calc\s+(?<Calc>[^\[\]]*)\s*\]", RegexOptions.IgnoreCase)]
    private static partial Regex CalcRegex();

    #endregion

    static readonly Dictionary<string, string> ms_parameters = [];

    static string ProcessRegex(string source, string key, Func<Regex> regex, Func<Group, string> fvalue, bool verbose = false) {
        regex().Matches(source).All(m => {
            string sourceValue = m.Captures[0].Value;
            if (m.Groups.TryGetValue(key, out Group g)) {
                string value = fvalue(g);
                source = source.Replace(sourceValue, value);
                if (verbose) { Console.WriteLine($"   {sourceValue} => {value}"); }
            }
            return true;
        });
        return source;
    }

    static string ProcessString(string source, UIGenCfg genCfg, bool full = false) {
        genCfg.Parameters.Keys.All(key => {
            string value = ms_parameters.GetValueOrDefault(key, key);
            source = Regex.Replace(source, @$"\[\s*(?<{key}>{key})\s*\]", value);
            if (full) { Console.WriteLine($"   [{key}] => {value}"); }
            return true;
        });
        source = ProcessRegex(source, "Iter", IterRegex, g => ms_parameters.GetValueOrDefault("i"), full);
        source = ProcessRegex(source, "Enum", EnumRegex, g => ms_parameters.GetValueOrDefault("Enum"), full);
        if (full) {
            source = ProcessRegex(source, "Calc", CalcRegex, g => $"{(int)g.Value.Compile(ms_parameters, new DotNetStandardMathContext())(ms_parameters)}");
            source = ProcessRegex(source, "LabelType", EQLabelRegex, g => $"{(int)Enum.Parse<EQLabelType>(g.Value)}");
            source = ProcessRegex(source, "GaugeType", EQGaugeRegex, g => $"{(int)Enum.Parse<EQGaugeType>(g.Value)}");
        }
        return source;
    }

    static bool ShouldGen(UIGenCfg gencfg, string outputFile) =>
        !File.Exists(outputFile) ||
        File.GetLastWriteTime(gencfg.TemplateFilename) > File.GetLastWriteTime(outputFile) ||
        File.GetLastWriteTime(gencfg.GenCfgPath) > File.GetLastWriteTime(outputFile);

    // processes an xmltemplate to produce multiple files
    static bool GenerateFiles(UIGenCfg gencfg) {
        bool result = false;

        // batch changes to give any previous writes time to finish
        BufferedCopyOp[] copyOps = new BufferedCopyOp[gencfg.CopyOps?.Length ?? 0];
        for (int i = 0; i < gencfg.CopyOps.Length; i++) {
            copyOps[i] = new BufferedCopyOp() {
                Target = gencfg.CopyOps[i].Target,
                Regexps = new string[gencfg.Count],
                Outputs = new string[gencfg.Count]
            };
            for (int j = gencfg.StartIndex; j < gencfg.Count; j++) {
                copyOps[i].Regexps[j] = gencfg.CopyOps[i].Regexp;
            }
        }

        for (int i = gencfg.StartIndex; i < gencfg.Count; i++) {
            string enumValue = gencfg.IsEnumerative ? gencfg.Enum[i - gencfg.StartIndex] : null;
            ms_parameters["i"] = $"{i}";
            if (gencfg.IsEnumerative) {
                ms_parameters["enum"] = enumValue;
                ms_parameters["Enum"] = enumValue;
            }
            gencfg.Parameters.All(kvp => {
                ms_parameters[kvp.Key] = kvp.Value[i - gencfg.StartIndex];
                return true;
            });

            string output = null, filename = ProcessString(gencfg.OutputFormat, gencfg);
            if (gencfg.ForceGeneration || ShouldGen(gencfg, filename)) {
                Console.WriteLine($"   Generating {filename}");
                output = File.ReadAllText(gencfg.TemplateFilename);
                output = ProcessString(output, gencfg, true);
                foreach (ReplaceOperation replaceOp in gencfg.ReplaceOps) {
                    if (replaceOp.Target.Contains(filename)) {
                        output = Regex.Replace(output, replaceOp.Regexp, replaceOp.Value);
                        // Console.WriteLine($"   replace ({filename}): {replaceOp.Regexp} => {replaceOp.Value}");
                    }
                }
                File.WriteAllText(filename, output);
            } else { Console.WriteLine($"   Skipping {filename}, file is newer than the template."); }
            result |= output != null;

            for (int j = 0; j < copyOps.Length; j++) {
                copyOps[j].Regexps[i] = ProcessString(copyOps[j].Regexps[i], gencfg);
                copyOps[j].Outputs[i] = (output != null) ? Regex.Match(output, copyOps[j].Regexps[i])?.Value : null;
            }
        }
        ms_parameters.Remove("i");
        ms_parameters.Remove("enum");
        ms_parameters.Remove("Enum");
        gencfg.Parameters.Keys.All(key => ms_parameters.Remove(key));

        // perform the batched copy operations
        foreach (BufferedCopyOp copyOp in copyOps) {
            string targetXml = File.ReadAllText(copyOp.Target);
            bool doWrite = false;
            for (int i = 0; i < copyOp.Regexps.Length; i++) {
                if (copyOp.Outputs[i] != null) {
                    targetXml = Regex.Replace(targetXml, copyOp.Regexps[i], copyOp.Outputs[i]);
                    // Console.WriteLine($"   copy ({copyOp.Target}): {copyOp.Regexps[i]} => {copyOp.Outputs[i]}");
                    doWrite = true;
                }
            }
            if (doWrite) {
                File.WriteAllText(copyOp.Target, targetXml);
            }
        }
        return result;
    }

    // ----------------------------------------------------------
    //  main
    // ----------------------------------------------------------

    static Program() {
        ms_parameters = [];
        foreach (string label in Enum.GetNames<EQLabelType>()) {
            ms_parameters[$"EQLabelType.{label}"] = $"{(int)Enum.Parse<EQLabelType>(label)}";
        }
        foreach (string gauge in Enum.GetNames<EQGaugeType>()) {
            ms_parameters[$"EQGaugeType.{gauge}"] = $"{(int)Enum.Parse<EQGaugeType>(gauge)}";
        }
    }

    static void Main(string[] args) {
        // step into the ui folder
        Environment.CurrentDirectory = Path.Combine(Environment.CurrentDirectory, "../");
        // scan through directories to find generator config files and process them accordingly
        string[] dirs = Directory.GetDirectories("./");
        foreach (string dir in dirs) {
            foreach (string file in Directory.GetFiles(dir, "*.uigencfg")) {
                UIGenCfg gencfg = new(file);
                gencfg.WriteConsole();
                GenerateFiles(gencfg);
            }
        }
        Console.WriteLine("Done!");
        //const string uiroot = @"..";
        //GenerateMerchant($"{uiroot}/D3PDA_MerchantWnd.xml");
    }

    // ----------------------------------------------------------
    //  old
    // ----------------------------------------------------------

    static string ProcessIndexer(string source, int i, char indexer) {
        string IndexPattern = @"\[" + indexer + @"(\+(?<value>-*[0-9]+))*\]";
        Match m;
        while ((m = Regex.Match(source, IndexPattern)).Success) {
            string strValue = m.Groups.TryGetValue("value", out Group g) ? g.Value : null;
            int value = string.IsNullOrEmpty(strValue) ? 0 : int.Parse(strValue);
            string newValue = $"{i + value}";
            source = source.Replace(m.Value, newValue);
            Console.WriteLine($"   {m.Value} => {newValue}");
        }
        return source;
    }

    [GeneratedRegex(@"CX\>(?<value>-*[0-9]+)")]
    private static partial Regex CXRegex();

    [GeneratedRegex(@"CY\>(?<value>-*[0-9]+)")]
    private static partial Regex CYRegex();

    static void GenerateMerchant(string filename) {
        const int SlotCount = 80;
        const int Columns = 2;

        string template = File.ReadAllText("templates/MerchantSlot.xml");
        Match mcx = CXRegex().Match(template), mcy = CYRegex().Match(template);
        int cx = 0, cy = 0;
        if (mcx.Success) {
            string strValue = null;
            if (mcx.Groups.TryGetValue("value", out Group g)) {
                strValue = g.Value;
            }
            cx = string.IsNullOrEmpty(strValue) ? 0 : int.Parse(strValue);
        }
        if (mcy.Success) {
            string strValue = null;
            if (mcy.Groups.TryGetValue("value", out Group g)) {
                strValue = g.Value;
            }
            cy = string.IsNullOrEmpty(strValue) ? 0 : int.Parse(strValue);
        }
        Console.WriteLine($"X: {cx}, Y: {cy}");

        StringBuilder output = new();
        for (int i = 0; i < SlotCount; i++) {
            int x = cx * (i % Columns);
            int y = cy * (i / Columns);
            string entry = ProcessIndexer(template, i, 'i');
            entry = ProcessIndexer(entry, x, 'x');
            entry = ProcessIndexer(entry, y, 'y');
            output.AppendLine(entry);
        }

        string result =
"<?xml version=\"1.0\"?>\n" +
"<XML ID=\"EQInterfaceDefinitionLanguage\">\n" +
"    <Schema xmlns=\"EverQuestData\" xmlns:dt=\"EverQuestDataTypes\"/>\n" +
"    <!-- -->\n" +
output.ToString() +
"    <!-- -->\n" +
"</XML>";
        File.WriteAllText(filename, result);
    }
}
