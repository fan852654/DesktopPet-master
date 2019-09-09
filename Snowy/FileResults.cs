using QueryEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snowy
{
    public partial class FileResults : Form
    {
        List<FileAndDirectoryEntry> list { get; set; }
        public FileResults(List<FileAndDirectoryEntry> list)
        {
            InitializeComponent();
            this.list = list;
        }

        private void FileResults_Load(object sender, EventArgs e)
        {
            label1.Text = "总共有"+list.Count + "个文件。";
            foreach(FileAndDirectoryEntry fade in list)
            {
                listBox1.Items.Add(fade.FullFileName);
            }
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            string filename = listBox1.SelectedItem.ToString();
            try
            {
                System.Diagnostics.Process.Start(filename);
            }
            catch
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select," + filename);
            }
        }
    }
}
