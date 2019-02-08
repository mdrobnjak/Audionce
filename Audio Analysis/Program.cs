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
            System.Threading.Tasks.Task.Run(()=>VisEnv.Run());

            using (AudioAnalyzerMDIForm analyzer = new AudioAnalyzerMDIForm())Application.Run(analyzer);
        }        
    }
}
