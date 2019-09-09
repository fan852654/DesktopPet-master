using EmailHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snowy
{
    public partial class EmailMessageResult : Form
    {
        public EmailConfig ec { get; set; }
        public MailMessage[] mm { get; set; } 
        public EmailMessageResult(EmailConfig ec)
        {
            InitializeComponent();
            this.ec = ec;
            listBox1.Items.Add("2");
        }

        private void EmailMessageResult_Load(object sender, EventArgs e)
        {
            mm = EmailTask.GetAllMailMessage(ec).ToArray();
            foreach(MailMessage mm1 in mm)
            {
                mm1.HeadersEncoding = Encoding.UTF8;
                listBox1.Items.Add(mm1.From.DisplayName + "：" + mm1.Subject);
            }
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            string namesub = ((ListBox)sender).SelectedItem.ToString();
            if (string.IsNullOrEmpty(namesub))
                return;
            int index = ((ListBox)sender).SelectedIndex;
            EmailMessageShow ems = new EmailMessageShow(mm[index]);
            ems.Show();
        }
    }
}
