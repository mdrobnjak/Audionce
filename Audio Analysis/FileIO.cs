using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalysis
{
    public partial class frmAudioAnalysis
    {
        private void cboSongNames_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string song = cboSongNames.Items[cboSongNames.SelectedIndex].ToString();
            ReadConfig(song);
        }

        private void btnSaveSong_Click(object sender, EventArgs e)
        {
            WriteConfig(cboSongNames.Text);
            LoadSongNames();
        }

        string[] config = new string[5];
        /*
         * 0. 1
         * 1. timer1.Interval
         * 2. rangeLows
         * 3. rangeHighs
         * 4. thresholds
         */
        const string configPath = @"..\..\..\Configs\";
        string currentConfig;

        private void WriteConfig(string fileName = "Default")
        {
            config[0] = "1";
            config[1] = timer1.Interval.ToString();

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

            System.IO.File.WriteAllLines(configPath + fileName + ".txt", config);
        }

        private void ReadConfig(string fileName = null)
        {
            if (fileName == null)
            {
                fileName = System.IO.File.ReadAllText(configPath + @"\LastConfig\LastConfig.txt");
                cboSongNames.Text = fileName;
            }

            currentConfig = fileName;

            config = System.IO.File.ReadAllLines(configPath + fileName + ".txt");

            //Int32.Parse(config[0]);
            timer1.Interval = Int32.Parse(config[1]);
            for (int i = 0; i < Range.Count; i++)
            {
                Ranges[i].LowCutIndex = Int32.Parse(config[2].Split(',')[i]);
                Ranges[i].HighCutIndex = Int32.Parse(config[3].Split(',')[i]);
                Ranges[i].Threshold = Int32.Parse(config[4].Split(',')[i]);
            }

            btnRange1_Click(null, null);
        }

        private void LoadSongNames()
        {
            cboSongNames.Items.Clear();

            string songName = "";
            foreach (string songPath in Directory.GetFiles(configPath))
            {
                songName = songPath.Replace(configPath, "");
                songName = songName.Replace(".txt", "");
                cboSongNames.Items.Add(songName);
            }
        }
    }
}
