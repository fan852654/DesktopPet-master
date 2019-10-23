using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snowy
{
    public partial class ChangeConfig : Form
    {
        public ChangeConfig()
        {
            InitializeComponent();
        }
        public ConfigHelper.ConfigHelper Config { get; set; }
        public static string EatfileAllow = string.Empty;
        private void ChangeConfig_Load(object sender, EventArgs e)
        {
            this.Config = Form1.Config;
            if (Config.GetConfig("EatFileEnable").ToString().Equals("true"))
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
            textBox1.Text = Config.GetConfig("EatFileAllowType").ToString();
            textBox2.Text = Config.GetConfig("EatFileNotAllowType").ToString();
            textBox3.Text = Config.GetConfig("EatBetween").ToString();
            textBox4.Text = Config.GetConfig("ListenPort").ToString();
            textBox5.Text = Config.GetConfig("DefDownloadSavePath") == null ? Environment.GetFolderPath(Environment.SpecialFolder.Programs) : Config.GetConfig("DefDownloadSavePath").ToString();
            EatfileAllow = Config.GetConfig("EatPath").ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Application.ExecutablePath;
                startInfo.Verb = "runas";
                System.Diagnostics.Process.Start(startInfo);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("已经是最高权限", "Warning", MessageBoxButtons.OK);
                return;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Config.SetConfig(new KeyValuePair<string, object>("EatFileAllowType", textBox1.Text));
            Config.SetConfig(new KeyValuePair<string, object>("EatFileNotAllowType", textBox2.Text));
            Config.SetConfig(new KeyValuePair<string, object>("EatBetween", textBox3.Text));
            Config.SetConfig(new KeyValuePair<string, object>("EatPath", EatfileAllow));
            Config.SetConfig(new KeyValuePair<string, object>("ListenPort", textBox4.Text));
            DirectoryInfo dir = new DirectoryInfo(textBox5.Text);
            if (!dir.Exists)
            {
                DialogResult dr = MessageBox.Show("路径不存在，是否尝试创建", "Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        dir.Create();
                    }
                    catch
                    {
                        MessageBox.Show("创建失败,此项目将不会保存", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Config.SetConfig(new KeyValuePair<string, object>("DefDownloadSavePath", textBox5.Text));
                }
                else if (dr == DialogResult.No)
                {
                    ;
                }
                else
                {
                    return;
                }
            }
            else
            {
                Config.SetConfig(new KeyValuePair<string, object>("DefDownloadSavePath", textBox5.Text));
            }
            Config.SaveConfig();
            MessageBox.Show("保存成功", "Message", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().Equals("否"))
            {
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
            }
            else
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                MessageBox.Show("修改成功", "OK", MessageBoxButtons.OK);
            }
        }
    }
}
