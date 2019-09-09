using FTPHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Snowy
{
    public partial class FTPResult : Form
    {
        private FTPHelper.FTPHelper ftpH { get; set; }
        private List<RouteDetil> FileList { get; set; }
        private List<RouteDetil> DirList { get; set; }
        private int NameSort = 0;
        private bool TypeSort = false;
        private bool LengthSort = false;
        private string PathRoute = string.Empty;
        private string savePath { get; set; }
        private string filename { get; set; }
        private bool isFugai { get; set; }
        public FTPResult(FTPHelper.FTPHelper ftp,string savePath,string filename,bool isFugai)
        {
            InitializeComponent();
            ftpH = ftp;
            listView1.ColumnClick += new ColumnClickEventHandler(ListView1Column_MouseClick);
            this.savePath = savePath;
            this.filename = filename;
            this.isFugai = isFugai;
        }

        private void FTPResult_Load(object sender, EventArgs e)
        {
            init();
        }
        private void init()
        {
            listView1.Items.Clear();
            textBox1.Text = ftpH.GetUrl();
            Uri u = new Uri(textBox1.Text);
            PathRoute = u.AbsolutePath;
            FileList = ftpH.GetFile();
            DirList = ftpH.GetDirctory();
            foreach (var str in DirList)
            {
                ListViewItem lvi = new ListViewItem(str.Name);
                lvi.SubItems.Add("文件夹");
                lvi.SubItems.Add(" ");
                lvi.SubItems.Add(str.ChangeTime);
                listView1.Items.Add(lvi); 
            }
            foreach (var str in FileList)
            {
                ListViewItem lvi = new ListViewItem(str.Name);
                lvi.SubItems.Add("文件");
                lvi.SubItems.Add(HumanReadableFilesize(str.Length));
                lvi.SubItems.Add(str.ChangeTime);
                listView1.Items.Add(lvi);
            }
        }
        private static String HumanReadableFilesize(double size)
        {
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size,2) + units[i];
        }
        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void ListView1Column_MouseClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 3)
                return;
            List<RouteDetil> li = new List<RouteDetil>();
            switch (e.Column)
            {
                case 0:
                    li.AddRange(FileList);
                    li.AddRange(DirList);
                    if (NameSort == 0)
                    {
                        li.Sort((x, y) => string.Compare(x.Name, y.Name));
                        NameSort++;
                    }else if (NameSort == 1)
                    {
                        li.Reverse();
                        NameSort++;
                    }else if(NameSort == 2)
                    {
                        NameSort = 0;
                    }
                    break;
                case 1:
                    if (TypeSort)
                    {
                        li.AddRange(FileList);
                        li.AddRange(DirList);
                    }
                    else
                    {
                        li.AddRange(DirList);
                        li.AddRange(FileList);
                    }
                    TypeSort = !TypeSort;
                    break;
                case 2:
                    List<RouteDetil> l = new List<RouteDetil>(FileList);
                    RouteDetil[] ll = l.ToArray();
                    Array.Sort(ll, new RouteDetilComparer());
                    if (LengthSort)
                    {
                        li.AddRange(ll);
                        li.AddRange(DirList);
                    }
                    else
                    {
                        li.AddRange(DirList);
                        li.AddRange(ll.Reverse());
                    }
                    LengthSort = !LengthSort;
                    break;
                default:
                    break;
            }
            listView1.Items.Clear();
            foreach (RouteDetil d in li)
            {
                ListViewItem lvi = new ListViewItem(d.Name);
                if (d.Type == RouteType.File)
                {
                    lvi.SubItems.Add("文件");
                    lvi.SubItems.Add(HumanReadableFilesize(d.Length));
                }
                else
                {
                    lvi.SubItems.Add("文件夹");
                    lvi.SubItems.Add(" ");
                }
                lvi.SubItems.Add(d.ChangeTime);
                listView1.Items.Add(lvi);
            }
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView lvi = (ListView)sender;
            string name = lvi.FocusedItem.Text;
            List<RouteDetil> li = new List<RouteDetil>();
            li.AddRange(FileList);
            li.AddRange(DirList);
            RouteDetil rd = li.Where(m => m.Name.Equals(name)).First();
            if(rd.Type == RouteType.Dir)
            {
                PathRoute += "/" + rd.Name;
                ftpH.SetPath(PathRoute);
                init();
            }
            else
            {
                DownloadHelper.DownloadHelper.StartFTPDownload(GetUrl(name), ftpH.GetUsr(), ftpH.GetPwd(), savePath, name, isFugai);
            }
        }
        public string GetUrl(string name)
        {
            if(ftpH.GetUrl().Substring(ftpH.GetUrl().Length -1,1).Equals("/"))
            {
                return ftpH.GetUrl() + name;
            }
            else
            {
                return ftpH.GetUrl() + "/" + name;
            }
        }
        private void FTPResult_Enter(object sender, EventArgs e)
        {
            Button1_Click(sender, e);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                Uri uri = new Uri(textBox1.Text);
                ftpH.SetPath(uri);
                init();
            }
            catch
            {
                MessageBox.Show("请输入合法的地址");
                return;
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
