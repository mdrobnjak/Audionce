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
        public static string Path { get; private set; }

        public static void WriteConfig(string filePath)
        {
            config[0] = "1";
            config[1] = "10";

            config[2] = config[3] = config[4] = "";

            for (int i = 0; i < Range.Count; i++)
            {
                config[2] += Range.Ranges[i].LowCutFreq + ",";
                config[3] += Range.Ranges[i].HighCutFreq + ",";
                config[4] += Range.Ranges[i].Threshold + ",";
            }
            for (int i = 2; i < 5; i++)
            {
                config[i] = config[i].TrimEnd(',');
            }

            System.IO.File.WriteAllLines(filePath, config);
        }

        public static string InitPathAndGetPreset()
        {
            InitPath();

            //ResetConfig();
            ReadConfig(Path + System.IO.File.ReadAllText(Path + @"\LastConfig\LastConfig.txt") + ".txt");

            return System.IO.File.ReadAllText(Path + @"\LastConfig\LastConfig.txt");
        }

        public static void InitPath()
        {
            string[] path = Environment.CurrentDirectory.Split('\\');
            Array.Resize(ref path, path.Count() - 3);
            Path = string.Join(@"\", path) + @"\Configs\";
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
            try
            {
                config = System.IO.File.ReadAllLines(filePath);

                //Int32.Parse(config[0]);
                //Int32.Parse(config[1]);
                for (int i = 0; i < Range.Count; i++)
                {
                    Range.Ranges[i].LowCutFreq = Int32.Parse(config[2].Split(',')[i]);
                    Range.Ranges[i].HighCutFreq = Int32.Parse(config[3].Split(',')[i]);
                    Range.Ranges[i].Threshold = Int32.Parse(config[4].Split(',')[i]);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not retrieve configuration of one or more ranges.");
            }
        }

        static void ResetConfig()
        {
            for (int i = 0; i < Range.Count; i++)
            {
                Range.Ranges[i].LowCutIndex = 0;
                Range.Ranges[i].HighCutIndex = 0;
                Range.Ranges[i].Threshold = 0;
            }
        }
    }
}
