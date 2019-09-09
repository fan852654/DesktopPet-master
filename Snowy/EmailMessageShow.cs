using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snowy
{
    public partial class EmailMessageShow : Form
    {
        public MailMessage mail;
        public EmailMessageShow(MailMessage mm)
        {
            InitializeComponent();
            mail = mm;
        }

        private void EmailMessageShow_Load(object sender, EventArgs e)
        {
            textBox1.Text = mail.Subject;
            textBox2.Text = mail.From.ToString();
            HtmlElement el = this.webBrowser1.Document.CreateElement(mail.Body);
            webBrowser1.Document.Body.AppendChild(el);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string foldPath = string.Empty;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择保存的文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foldPath = dialog.SelectedPath;
            }
            MessageBox.Show("正在保存");
            foreach (Attachment fujian in mail.Attachments)
            {
                using (FileStream fs = new FileStream(Path.Combine(foldPath, fujian.Name), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fujian.ContentStream.CopyTo(fs);
                }
            }
            MessageBox.Show("保存结束，已保存至" + foldPath);
        }
    }
}
