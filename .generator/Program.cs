partial class Program {
    static readonly string[,] StatListL = new string[,] { 
        // Name, EQType
        { "STR", "5" },
        { "STA", "6" },
        { "AGI", "8"},
        { "DEX", "7" },
        { "WIS", "9" },
        { "INT", "10" },
        { "CHA", "11" }
    };

    static readonly string[,] StatListR = new string[,] {
        // Name, EQType
        { "AC", "22" },
        { "AT", "23" },
        { "MR", "16"},
        { "FR", "14" },
        { "CR", "15"},
        { "PR", "12" },
        { "DR", "13" }
    };

    static string ReplaceLabelTypes(string source) {
        const string EQLabelPattern = @"\[EQLabelType.(?<name>[a-zA-Z0-9]+)\]";
        Match m;
        while ((m = Regex.Match(source, EQLabelPattern)).Success) {
            string name = m.Groups.GetValueOrDefault("name").Value;
            int value = (int)Enum.Parse(typeof(EQLabelType), name);
            source = source.Replace(m.Value, value.ToString());
            Console.WriteLine($"   {m.Value} => {value}");
        }
        return source;
    }

    static string ReplaceGaugeTypes(string source) {
        const string EQGaugePattern = @"\[EQGaugeType.(?<name>[a-zA-Z0-9]+)\]";
        Match m;
        while ((m = Regex.Match(source, EQGaugePattern)).Success) {
            string name = m.Groups.GetValueOrDefault("name").Value;
            int value = (int)Enum.Parse(typeof(EQGaugeType), name);
            source = source.Replace(m.Value, value.ToString());
            Console.WriteLine($"   {m.Value} => {value}");
        }
        return source;
    }

    static string ProcessEQTypes(string source) {
        source = ReplaceLabelTypes(source);
        source = ReplaceGaugeTypes(source);
        return source;
    }

    static string ProcessIndexer(string source, int i, char indexer) {
        string IndexPattern = @"\[" + indexer + @"(\+(?<value>-*[0-9]+))*\]";
        Match m;
        while ((m = Regex.Match(source, IndexPattern)).Success) {
            string strValue;
            if (m.Groups.TryGetValue("value", out Group g)) {
                strValue = g.Value;
            }
            else {
                strValue = "0";
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
            string output = original.Replace("[name]", statList[i, 0])
                                    .Replace("[eqtype]", statList[i, 1]);
            File.WriteAllText(outFilename, output);
        }
    }

    static void GenerateMerchant(string filename) {
        string template = File.ReadAllText("templates/MerchantSlot.xml");
    }

    static void Main(string[] args) {
        const string uiroot = @"../../../";
        GenerateBuffs($"{uiroot}/l_buff_window/Buff.xml");
        GenerateBuffs($"{uiroot}/r_buff_window/Buff.xml");
        GenerateGroup($"{uiroot}/group_window/Group.xml");
        GenerateSpells($"{uiroot}/spell_window/Spell.xml");
        //GenerateStats($"{backref}/status_window/StatL.xml", StatListL);
        //GenerateStats($"{backref}/status_window/StatR.xml", StatListR);
    }
}


