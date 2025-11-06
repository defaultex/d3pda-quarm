using System.Text;

partial class Program {
    
    [GeneratedRegex(@"\[EQLabelType.(?<name>[a-zA-Z0-9]+)\]")]
    private static partial Regex EQLabelRegex();

    [GeneratedRegex(@"\[EQGaugeType.(?<name>[a-zA-Z0-9]+)\]")]
    private static partial Regex EQGaugeRegex();

    // parses i, i+value, i-value, i*value, i/value, and i%value
    [GeneratedRegex(@"\[i((?<op>[\/\+\-\*\%]{1})(?<value>\d+))*\]")]
    private static partial Regex IndexRegex();

    static string ReplaceLabelTypes(string source) {
        Match m;
        while ((m = EQLabelRegex().Match(source)).Success) {
            string name = m.Groups.GetValueOrDefault("name").Value;
            int value = (int)Enum.Parse(typeof(EQLabelType), name);
            source = source.Replace(m.Value, value.ToString());
            Console.WriteLine($"   {m.Value} => {value}");
        }
        return source;
    }

    static string ReplaceGaugeTypes(string source) {
        Match m;
        while ((m = EQGaugeRegex().Match(source)).Success) {
            string name = m.Groups.GetValueOrDefault("name").Value;
            int value = (int)Enum.Parse(typeof(EQGaugeType), name);
            source = source.Replace(m.Value, value.ToString());
            Console.WriteLine($"   {m.Value} => {value}");
        }
        return source;
    }

    static string ProcessIndices(string source, int i) {
        Match m;
        while ((m = IndexRegex().Match(source)).Success) {
            string strValue = null, strOp = null;
            if (m.Groups.TryGetValue("value", out Group g0)) {
                strValue = g0.Value;
            }
            if (m.Groups.TryGetValue("op", out Group g1)) {
                strOp = g1.Value;
            } 
            int value = string.IsNullOrWhiteSpace(strValue) ? 0 : int.Parse(strValue);
            string newValue = strOp switch {
                "+" => $"{i + value}",
                "-" => $"{i - value}",
                "*" => $"{i * value}",
                "/" => $"{i / value}",
                "%" => $"{i % value}",
                _ => $"{i}"
            }; 
            source = source.Replace(m.Value, newValue);
            Console.WriteLine($"   {m.Value} => {newValue}");
        }
        return source;
    }

    static string ProcessFile(string source, Func<string, string> proc = null) {
        string output = (proc != null) ? proc(source) : source;
        output = ReplaceLabelTypes(output);
        output = ReplaceGaugeTypes(output);
        return output;
    }

    static bool ShouldGen(string templatePath, string outputFile) =>
        !File.Exists(outputFile) || File.GetLastWriteTime(templatePath) > File.GetLastWriteTime(outputFile);

    // process an xmltemplate to produce a file from it
    static bool GenerateFile(string templatePath, string outFilename, Func<string, string> proc = null) {
        if (ShouldGen(templatePath, outFilename)) {
            string original = File.ReadAllText(templatePath);
            Console.WriteLine($"   Generating {outFilename}");
            string output = ProcessFile(original, proc);
            File.WriteAllText(outFilename, output);
            return true;
        } else {
            Console.WriteLine($"   Skipping {outFilename}, file is newer than the template.");
            return false;
        }
    }

    // processes an xmltemplate to produce 'count - offset' amount of files from it
    static bool GenerateFiles(UIGenCfg gencfg) {
        bool result = false;
        string original = File.ReadAllText(gencfg.TemplateFilename);
        for (int i = gencfg.StartIndex; i < gencfg.Count; i++) {
            string outFilename = string.Format(gencfg.OutputFormat, i);
            if (ShouldGen(gencfg.TemplateFilename, outFilename)) {
                Console.WriteLine($"   Generating {outFilename}");
                string output = ProcessIndices(original, i);
                output = ProcessFile(output);
                File.WriteAllText(outFilename, output);
                result = true;
            } else {
                Console.WriteLine($"   Skipping {outFilename}, file is newer than the template.");
            }
        }
        return result;
    }

    // ----------------------------------------------------------
    //  special cases
    // ----------------------------------------------------------

    static void GenStatWindow() {
        foreach (string stat in new[] {
            "STR",
            "STA",
            "AGI",
            "DEX",
            "WIS",
            "INT",
            "CHA",
            "XP",
            "AA" }) {
            GenerateFile("./status_window/StatL.xmltemplate", $"./status_window/{stat}.xml",
                (source) => source.Replace("[Name]", stat));
        }
        foreach (string stat in new[] {
            "AC",
            "AT",
            "MR",
            "FR",
            "CR",
            "PR",
            "DR"}) {
            GenerateFile("./status_window/StatR.xmltemplate", $"./status_window/{stat}.xml",
                (source) => source.Replace("[Name]", stat));
        }
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
            string[] files = Directory.GetFiles(dir, "*.uigencfg");
            if (files == null || files.Length < 1) { continue; }
            UIGenCfg gencfg = new(files[0]);
            gencfg.WriteConsole();
            GenerateFiles(gencfg);
        }

        // specialized output
        GenStatWindow();

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
            string strValue = null;
            if (m.Groups.TryGetValue("value", out Group g)) {
                strValue = g.Value;
            }
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
