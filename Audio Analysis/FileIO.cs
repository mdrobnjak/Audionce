using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public static class FileIO
    {
        public static Range[] Ranges;
                
        public static string Path { get; private set; }

        public static void WriteConfig(string filePath)
        {
            config[0] = "1";
            config[1] = "10";

            config[2] = config[3] = config[4] = "";

            for (int i = 0; i < Range.Count; i++)
            {
                config[2] += Ranges[i].LowCutIndex + ",";
                config[3] += Ranges[i].HighCutIndex + ",";
                config[4] += (int)Ranges[i].Threshold + ",";
            }
            for (int i = 2; i < 5; i++)
            {
                config[i] = config[i].TrimEnd(',');
            }

            System.IO.File.WriteAllLines(filePath, config);
        }

        public static string InitPathAndGetPreset()
        {
            string[] path = Environment.CurrentDirectory.Split('\\');
            Array.Resize(ref path, path.Count() - 3);
            Path = string.Join(@"\", path) + @"\Configs\";

            ReadConfig(Path + System.IO.File.ReadAllText(Path + @"\LastConfig\LastConfig.txt") + ".txt");

            return System.IO.File.ReadAllText(Path + @"\LastConfig\LastConfig.txt");
        }

        static string[] config = new string[5];
        /*
         * 0. 1
         * 1. 10
         * 2. rangeLows
         * 3. rangeHighs
         * 4. thresholds
         */

        public static void ReadConfig(string filePath)
        {
            config = System.IO.File.ReadAllLines(filePath);

            //Int32.Parse(config[0]);
            //Int32.Parse(config[1]);
            for (int i = 0; i < Range.Count; i++)
            {
                Ranges[i].LowCutIndex = Int32.Parse(config[2].Split(',')[i]);
                Ranges[i].HighCutIndex = Int32.Parse(config[3].Split(',')[i]);
                Ranges[i].Threshold = Int32.Parse(config[4].Split(',')[i]);
            }            
        }
    }    
}
