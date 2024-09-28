using System.Text;

partial class Program {
    static readonly string[,] StatListL = new string[,] {
        // Name, EQType
        { "STR", "5" },
        { "STA", "6" },
        { "AGI", "8" },
        { "DEX", "7" },
        { "WIS", "9" },
        { "INT", "10" },
        { "CHA", "11" }
    };

    static readonly string[,] StatListR = new string[,] {
        // Name, EQType
        { "AC", "22" },
        { "AT", "23" },
        { "MR", "16" },
        { "FR", "14" },
        { "CR", "15" },
        { "PR", "12" },
        { "DR", "13" }
    };

    [GeneratedRegex(@"\[EQLabelType.(?<name>[a-zA-Z0-9]+)\]")]
    private static partial Regex EQLabelRegex();

    [GeneratedRegex(@"\[EQGaugeType.(?<name>[a-zA-Z0-9]+)\]")]
    private static partial Regex EQGaugeRegex();

    [GeneratedRegex(@"CX\>(?<value>-*[0-9]+)")]
    private static partial Regex CXRegex();

    [GeneratedRegex(@"CY\>(?<value>-*[0-9]+)")]
    private static partial Regex CYRegex();

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

    static string ProcessEQTypes(string source) => ReplaceGaugeTypes(ReplaceLabelTypes(source));

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

    static void GenerateBuffs(string filename, int h = 34) {
        const int BuffCount = 15;

        string original = File.ReadAllText(filename);
        for (int i = 0; i < BuffCount; i++) {
            string outFilename = filename.Replace(".xml", $"{i}.xml");
            Console.WriteLine($"Generating {outFilename}");

            string output = ProcessIndexer(original, i, 'i');
            output = ProcessIndexer(output, 0, 'x');
            output = ProcessIndexer(output, i * h, 'y');
            output = ProcessEQTypes(output);
            File.WriteAllText(outFilename, output);
        }
    }

    static void GenerateSpells(string filename, int h = 30) {
        const int SpellCount = 8;

        string original = File.ReadAllText(filename);
        for (int i = 0; i < SpellCount; i++) {
            string outFilename = filename.Replace(".xml", $"{i}.xml");
            Console.WriteLine($"Generating {outFilename}");
            string output = ProcessIndexer(original, i, 'i');
            output = ProcessIndexer(output, 0, 'x');
            output = ProcessIndexer(output, i * h, 'y');
            output = ProcessEQTypes(output);
            File.WriteAllText(outFilename, output);
        }
    }

    static void GenerateGroup(string filename, int h = 29) {
        const int GroupCount = 5;

        string original = File.ReadAllText(filename);
        for (int i = 0; i < GroupCount; i++) {
            string outFilename = filename.Replace(".xml", $"{i + 1}.xml");
            Console.WriteLine($"Generating {outFilename}");
            string output = ProcessIndexer(original, i, 'i');
            output = ProcessIndexer(output, i * h, 'y');
            output = ProcessEQTypes(output);
            File.WriteAllText(outFilename, output);
        }
    }

    static void GenerateStats(string filename, string[,] statList) {
        string original = File.ReadAllText(filename);
        for (int i = 0; i < statList.Length; i++) {
            string outFilename = filename.Replace(".xml", $"{i + 1}.xml");
            Console.WriteLine($"Generating {outFilename}");
            string output = original
                .Replace("[name]", statList[i, 0])
                .Replace("[eqtype]", statList[i, 1]);
            File.WriteAllText(outFilename, output);
        }
    }

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

    static void Main(string[] args) {
        const string uiroot = @"..";

        //GenerateBuffs($"{uiroot}/l_buff_window/Buff.xml");
        //GenerateBuffs($"{uiroot}/r_buff_window/Buff.xml");
        //GenerateGroup($"{uiroot}/group_window/Group.xml");
        //GenerateSpells($"{uiroot}/spell_window/Spell.xml");
        //GenerateStats($"{uiroot}/status_window/StatL.xml", StatListL);
        //GenerateStats($"{uiroot}/status_window/StatR.xml", StatListR);
        //GenerateMerchant($"{uiroot}/D3PDA_MerchantWnd.xml");
    }
}
