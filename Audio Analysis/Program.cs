using Microsoft.ML;
using System;
using System.IO;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (AudioAnalyzerMDIForm myapps = new AudioAnalyzerMDIForm())
            {
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(myapps);
            }
        }        
    }
}
