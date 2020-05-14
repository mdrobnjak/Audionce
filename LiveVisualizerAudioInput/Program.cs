using Microsoft.ML;
using System;
using System.IO;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using System.Windows.Forms;

namespace LiveVisualizerAudioInput
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (LiveVisualizerAudioInputMDIForm analyzer = new LiveVisualizerAudioInputMDIForm())
            {
                Application.Run(analyzer);
            }
        }        
    }
}
