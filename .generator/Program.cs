using System.Diagnostics;
using MathEvaluation.Context;
using MathEvaluation.Extensions;

partial class Program {
    [GeneratedRegex(@"\[\s*calc\s+(?<Calc>[^\[\]]*)\s*\]", RegexOptions.IgnoreCase)]
    private static partial Regex CalcRegex();

    static readonly Dictionary<string, string> ms_parameters = [];

    static Program() {
        ms_parameters = [];
        foreach (string label in Enum.GetNames<EQLabelType>()) {
            ms_parameters[$"EQLabelType.{label}"] = $"{(int)Enum.Parse<EQLabelType>(label)}";
        }
        foreach (string gauge in Enum.GetNames<EQGaugeType>()) {
            ms_parameters[$"EQGaugeType.{gauge}"] = $"{(int)Enum.Parse<EQGaugeType>(gauge)}";
        }
    }

    static string ReplaceParameters(string source, bool verbose = false) {
        string result = source;
        foreach (string key in ms_parameters.Keys) {
            string value = ms_parameters[key];
            foreach (Match m in Regex.Matches(result, @$"\[\s*(?<Param>{key})\s*\]")) {
                result = result.Replace(m.Value, value);
                Debug.WriteLineIf(verbose, $"   parameter: [{key}] => {value}");
            }
        }
        return result;
    }

    static string ProcessString(string source, bool verbose = false) {
        string result = source;
        result = ReplaceParameters(result, verbose);
        foreach (Match m in CalcRegex().Matches(result)) {
            if (m.Groups.TryGetValue("Calc", out Group g)) {
                string value = $"{g.Value.Evaluate(ms_parameters, new DotNetStandardMathContext())}";
                result = result.Replace(m.Captures[0].Value, value);
                Debug.WriteLineIf(verbose, $"   calc: [{g.Value}] => {value}");
            }
        }
        result = ReplaceParameters(result, verbose);
        return result;
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
            ms_parameters["i"] = $"{i}";
            gencfg.Parameters.All(kvp => {
                ms_parameters[kvp.Key] = kvp.Value[i - gencfg.StartIndex];
                return true;
            });

            string output = null, filename = ProcessString(gencfg.OutputFormat);
            if (gencfg.ForceGeneration || ShouldGen(gencfg, filename)) {
                Console.WriteLine($"   Generating {filename}");
                output = File.ReadAllText(gencfg.TemplateFilename);
                output = ProcessString(output, true);
                foreach (ReplaceOperation replaceOp in gencfg.ReplaceOps) {
                    if (replaceOp.Target.Contains(filename)) {
                        output = Regex.Replace(output, replaceOp.Regexp, replaceOp.Value);
                        Console.WriteLine($"   replace ({filename}): {replaceOp.Regexp} => {replaceOp.Value}");
                    }
                }
                File.WriteAllText(filename, output);
            }
            // else { Console.WriteLine($"   Skipping {filename}, file is newer than the template."); }
            result |= output != null;

            for (int j = 0; j < copyOps.Length; j++) {
                copyOps[j].Regexps[i] = ProcessString(copyOps[j].Regexps[i]);
                copyOps[j].Outputs[i] = (output != null) ? Regex.Match(output, copyOps[j].Regexps[i])?.Value : null;
            }
        }
        ms_parameters.Remove("i");
        gencfg.Parameters.Keys.All(key => ms_parameters.Remove(key));

        // perform the batched copy operations
        foreach (BufferedCopyOp copyOp in copyOps) {
            string targetXml = File.ReadAllText(copyOp.Target);
            bool doWrite = false;
            for (int i = 0; i < copyOp.Regexps.Length; i++) {
                if (copyOp.Outputs[i] != null) {
                    targetXml = Regex.Replace(targetXml, copyOp.Regexps[i], copyOp.Outputs[i]);
                    Console.WriteLine($"   copy ({copyOp.Target}): {copyOp.Regexps[i]} => {copyOp.Outputs[i]?.Split('\n')[0]}");
                    doWrite = true;
                }
            }
            if (doWrite) { File.WriteAllText(copyOp.Target, targetXml); }
        }
        return result;
    }

    // ----------------------------------------------------------
    //  main
    // ----------------------------------------------------------

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
    }
}
