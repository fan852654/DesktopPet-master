using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snowy
{
    public partial class FastFindFile : Form
    {
        public FastFindFile()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (textBox1.Text.Trim().Equals(""))
            {
                MessageBox.Show("请输入文件包含的名字", "Error", MessageBoxButtons.OK);
                button1.Enabled = true;
            }
            else
            {
                Search();
            }
        }
        public int Search()
        {
            List<QueryEngine.FileAndDirectoryEntry> filteredResult = Form1.entries
                        .Where(f => f.FileName.ToUpper().Contains(textBox1.Text))
                        .OrderBy(f => f.FileName)
                        .ToList();
            FileResults fr = new FileResults(filteredResult);
            fr.Show();
            this.Close();
            return 1;
        }
    }
}
