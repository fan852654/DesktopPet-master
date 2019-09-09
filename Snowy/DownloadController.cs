using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DownloadHelper;
using System.Linq;
using System.Diagnostics;

namespace Snowy
{
    public partial class DownloadController : Form
    {
        NotifyIcon ni;
        public DownloadController(NotifyIcon ni,string defpath)
        {
            InitializeComponent();
            listViewEx1.ProgressTextColor = Color.Black;
            listViewEx1.ProgressColor = Color.YellowGreen;
            this.ni = ni;
            if (string.IsNullOrEmpty(defpath) || !Directory.Exists(defpath))
            {
                textBox3.Text = Environment.GetFolderPath(Environment.SpecialFolder.Programs); ;
                textBox8.Text = Environment.GetFolderPath(Environment.SpecialFolder.Programs); ;
            }
            else
            {
                textBox3.Text = defpath;
                textBox8.Text = defpath;
            }
            timer1.Interval = 1000;
        }

        private void DownloadController_Load(object sender, EventArgs e)
        {
            init();
            timer1.Start();
        }
        public void init()
        {
            listViewEx1.Items.Clear();
            Dictionary<DownloadJobInfo, int> jobs = DownloadHelper.DownloadHelper.GetDownloadJobs();
            foreach(KeyValuePair<DownloadJobInfo,int> kvp in jobs)
            {
                ListViewItem lvi = new ListViewItem
                {
                    Name = kvp.Key.GetMD5(),
                    Text = kvp.Key.filename
                };
                lvi.SubItems.Add(kvp.Value.ToString());
                if (kvp.Value == 0)
                {
                    lvi.SubItems.Add("开始中...");
                } else if (kvp.Value > 0 && kvp.Value < 100)
                {
                    lvi.SubItems.Add("下载中...");
                }
                else if (kvp.Value == 100 && File.Exists(Path.Combine(kvp.Key.path,kvp.Key.filename)))
                {
                    lvi.SubItems.Add("已完成！");
                }
                else
                {
                    lvi.SubItems.Add("请稍后重试。");
                }
                listViewEx1.Items.Add(lvi);
            }
        }
        public void updateList()
        {
            Dictionary<DownloadJobInfo, int> jobbak = DownloadHelper.DownloadHelper.GetDownloadJobs();
            DownloadJobInfo[] djilist = jobbak.Keys.ToArray();
            int[] intlist = jobbak.Values.ToArray();
            for(int i = 0; i < djilist.Length; i++)
            {
                KeyValuePair<DownloadJobInfo, int> kvp = new KeyValuePair<DownloadJobInfo, int>(djilist[i], intlist[i]);
                bool fla = false;
                foreach (ListViewItem item in listViewEx1.Items)
                {
                    if (item.Name == kvp.Key.GetMD5())
                    {
                        fla = true;
                        item.SubItems[1].Text = kvp.Value.ToString();
                        if (kvp.Value == 0)
                        {
                            item.SubItems[2].Text = "开始中...";
                        }
                        else if (kvp.Value > 0 && kvp.Value < 100)
                        {
                            item.SubItems[2].Text = "下载中...";
                        }
                        else if (kvp.Value == 100 && File.Exists(Path.Combine(kvp.Key.path, kvp.Key.filename)))
                        {
                            item.SubItems[2].Text = "已完成！";
                        }
                        else
                        {
                            item.SubItems[2].Text = "请稍后重试。";
                        }
                        break;
                    }
                }
                if (!fla)
                {
                    ListViewItem lvi = new ListViewItem
                    {
                        Name = kvp.Key.GetMD5(),
                        Text = kvp.Key.filename
                    };
                    lvi.SubItems.Add(kvp.Value.ToString());
                    if (kvp.Value == 0)
                    {
                        lvi.SubItems.Add("开始中...");
                    }
                    else if (kvp.Value > 0 && kvp.Value < 100)
                    {
                        lvi.SubItems.Add("下载中...");
                    }
                    else if (kvp.Value == 100 && File.Exists(Path.Combine(kvp.Key.path, kvp.Key.filename)))
                    {
                        lvi.SubItems.Add("已完成！");
                    }
                    else
                    {
                        lvi.SubItems.Add("请稍后重试。");
                    }
                    listViewEx1.Items.Add(lvi);
                }
            }
            #region 弃用
            //foreach (KeyValuePair<DownloadJobInfo, int> kvp in jobbak)
            //{
            //    bool fla = false;
            //    foreach (ListViewItem item in listViewEx1.Items)
            //    {
            //        if (item.Name == kvp.Key.GetMD5())
            //        {
            //            fla = true;
            //            item.SubItems[1].Text = kvp.Value.ToString();
            //            if (kvp.Value == 0)
            //            {
            //                item.SubItems[2].Text = "开始中...";
            //            }
            //            else if (kvp.Value > 0 && kvp.Value < 100)
            //            {
            //                item.SubItems[2].Text = "下载中...";
            //            }
            //            else if (kvp.Value == 100 && File.Exists(Path.Combine(kvp.Key.path, kvp.Key.filename)))
            //            {
            //                item.SubItems[2].Text = "已完成！";
            //            }
            //            else
            //            {
            //                item.SubItems[2].Text = "请稍后重试。";
            //            }
            //            break;
            //        }
            //    }
            //    if (!fla)
            //    {
            //        ListViewItem lvi = new ListViewItem
            //        {
            //            Name = kvp.Key.GetMD5(),
            //            Text = kvp.Key.filename
            //        };
            //        lvi.SubItems.Add(kvp.Value.ToString());
            //        if (kvp.Value == 0)
            //        {
            //            lvi.SubItems.Add("开始中...");
            //        }
            //        else if (kvp.Value > 0 && kvp.Value < 100)
            //        {
            //            lvi.SubItems.Add("下载中...");
            //        }
            //        else if (kvp.Value == 100 && File.Exists(Path.Combine(kvp.Key.path, kvp.Key.filename)))
            //        {
            //            lvi.SubItems.Add("已完成！");
            //        }
            //        else
            //        {
            //            lvi.SubItems.Add("请稍后重试。");
            //        }
            //        listViewEx1.Items.Add(lvi);
            //    }
            //}
            #endregion
        }
        private void ListViewEx1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            listViewEx1.ProgressColumIndex = 1;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            string name = textBox2.Text;
            string path = textBox3.Text;
            Uri uri;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("请选择有效的保存路径");
                return;
            }
            try
            {
                uri = new Uri(url);
                if (uri.IsFile)
                {
                    MessageBox.Show("请使用HTTP链接方式或者使用其他方式下载");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("请输入合法的URL地址");
                return;
            }
            if (File.Exists(Path.Combine(path, name)) && !checkBox2.Checked)
            {
                MessageBox.Show("文件已经存在，请重新选择");
                return;
            }
            else if(File.Exists(Path.Combine(path, name)) && checkBox2.Checked)
            {
                File.Delete(Path.Combine(path, name));
            }
            DownloadHelper.DownloadHelper.StartHttpDownload(uri, path, name,checkBox2.Checked);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = dialog.SelectedPath;                
            }
        }

        private void TextBox3_DoubleClick(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox3.Text))
            {
                System.Diagnostics.Process.Start("explorer.exe", textBox3.Text);
            }
            else
            {
                MessageBox.Show("不是有效的文件路径");
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            updateList();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DownloadHelper.DownloadHelper.RemoveAllData();
            listViewEx1.Items.Clear();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                Uri uri = new Uri(tb.Text);
                if (uri.IsFile)
                    return;
                if (uri.IsAbsoluteUri)
                {
                    textBox2.Text = tb.Text.Split('/')[tb.Text.Split('/').Count() - 1];
                }
            }catch
            { return; }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox8.Text = dialog.SelectedPath;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            string username = textBox4.Text;
            string password = textBox5.Text;
            string url = textBox6.Text;
            string filename = textBox7.Text;
            string path = textBox8.Text;
            try
            {
                Uri uri = new Uri(url);
            }catch(Exception ex)
            {
                MessageBox.Show("出错了:" + ex.Message);
            }
            if (!Directory.Exists(filename))
                Directory.CreateDirectory(filename);
            DownloadHelper.DownloadHelper.StartFTPDownload(url, username, password, path, filename, checkBox1.Checked);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            string user = textBox4.Text;
            string pwd = textBox5.Text;
            string url = textBox6.Text;
            try
            {
                Uri uri = new Uri(url);
            }catch
            { 
                MessageBox.Show("请输入合法的FTP路径，如：ftp://x.x.x.x/");
                return;
            }
            FTPHelper.FTPHelper fhelper = new FTPHelper.FTPHelper(url, user, pwd);
            FTPResult ftpr = new FTPResult(fhelper,textBox8.Text,textBox7.Text,checkBox1.Checked);
            ftpr.Show();
        }
        //打开文件
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listViewEx1.SelectedItems.Count == 0)
                return;
            ListView.SelectedListViewItemCollection slvi = listViewEx1.SelectedItems;
            Dictionary<DownloadJobInfo, int> jobbak = DownloadHelper.DownloadHelper.GetDownloadJobs();
            foreach (ListViewItem li in slvi)
            {
                try
                {
                    var list = jobbak.Where(m => m.Key.filename == li.Text && m.Key.GetMD5() == li.Name).First();
                    string path = Path.Combine(list.Key.path, list.Key.filename);
                    if (File.Exists(path))
                    {
                        Process.Start(path);
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
        //打开文件夹
        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listViewEx1.SelectedItems.Count == 0)
                return;
            ListView.SelectedListViewItemCollection slvi = listViewEx1.SelectedItems;
            Dictionary<DownloadJobInfo, int> jobbak = DownloadHelper.DownloadHelper.GetDownloadJobs();
            foreach (ListViewItem li in slvi)
            {
                try
                {
                    var list = jobbak.Where(m => m.Key.filename == li.Text && m.Key.GetMD5() == li.Name).First();
                    string path = Path.Combine(list.Key.path, list.Key.filename);
                    if (File.Exists(path))
                    {
                        Process.Start("explorer.exe", "/select," + path);
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
