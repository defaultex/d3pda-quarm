partial class Program {
    static void Main(string[] args) {
        // step into the ui folder
        Environment.CurrentDirectory = Path.Combine(Environment.CurrentDirectory, "../");

        //UIGenCfg labelConfig = new(".generator/templates/Label.labelcfg");
        //Console.WriteLine("Done!");

        // scan through directories to find generator config files and process them accordingly
        string[] dirs = Directory.GetDirectories("./");
        foreach (string dir in dirs) {
            foreach (string file in Directory.GetFiles(dir, "*.uigencfg")) {
                UIGenCfg gencfg = new(file);
                gencfg.WriteConsole();
                gencfg.GenerateFiles();
            }
        }
        Console.WriteLine("Done!");
    }
}
