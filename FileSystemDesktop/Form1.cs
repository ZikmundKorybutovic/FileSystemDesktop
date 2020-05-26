using FileSystemAnalyzer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FileSystemAnalyzer.Analyzer;

namespace FileSystemDesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void analyzeButton_Click(object sender, EventArgs e)
        {
            var analyzer = new Analyzer(tbDirectory.Text);
            var result = analyzer.DoAnalysis();
            tbResult.Text = formatResult(result);
        }

        private string formatResult(AnalysisResult result)
        {
            if (result.DeletedFolders == null)
            {
                return result.Comment;
            }

            string formattedResult = "";

            foreach (var file in result.AddedFiles)
            {
                formattedResult += $"[A] {file.Name}{Environment.NewLine}";
            }

            foreach (var file in result.ModifiedFiles)
            {
                formattedResult += $"[M] {file.Name} (verze {file.Version}){Environment.NewLine}";
            }

            foreach (var file in result.DeletedFiles)
            {
                formattedResult += $"[D] {file.Name}{Environment.NewLine}";
            }

            foreach (var folder in result.DeletedFolders)
            {
                formattedResult += $"[D] {folder.Name}";
            }

            return formattedResult;
        }
    }
}
