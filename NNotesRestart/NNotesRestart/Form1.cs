using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace NNotesRestart
{
    public partial class Form1 : Form
    {
        static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string localNNotesPath = localPath + "\\NNotes";
        string textFilePath = localNNotesPath + "\\Text.txt";
        static string exePath = Application.StartupPath;
        string exeFilePath = exePath + "\\NNotes.exe";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(textFilePath))
            {
                string text = File.ReadAllText(textFilePath);
                if (text != "")
                {
                    try
                    {
                        Process nnotes = new Process();
                        nnotes.StartInfo.FileName = exeFilePath;
                        nnotes.Start();
                    }
                    catch
                    {
                        MessageBox.Show("Could not start NNotes! Please put NNotesRestart.exe back in the folder with NNotes.exe!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            Application.Exit();

        }
    }
}
