using System;
using System.Windows.Forms;

namespace Snowy
{
    public partial class AddEmail : Form
    {
        public bool IsChange = false;
        protected EmailHelper.EmailConfig ecbak { get; set; }
        public AddEmail()
        {
            InitializeComponent();
            //textBox1.Text = "outlook.office365.com";
            //textBox2.Text = "993";
            //textBox3.Text = "ghost980@outlook.com";
            ////textBox4.Text = "ghhrwcrlkoshihhc";
            //textBox4.Text = "fanzs900839";
            Text = "添加邮箱配置";
        }
        public AddEmail(EmailHelper.EmailConfig ec)
        {
            InitializeComponent();
            Text = "修改邮箱配置";
            textBox1.Text = ec.Url;
            textBox2.Text = ec.Port.ToString();
            textBox3.Text = ec.UserName;
            textBox4.Text = ec.PassWrod;
            checkBox1.Checked = ec.SSl;
            if (ec.EmailType == EmailHelper.EmailType.IMAP)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
            IsChange = true;
            ecbak = ec;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string server = textBox1.Text;
            string port = textBox2.Text;
            string username = textBox3.Text;
            string password = textBox4.Text;
            bool isTSL = checkBox1.Checked;
            EmailHelper.EmailType et = radioButton1.Checked ? EmailHelper.EmailType.IMAP : EmailHelper.EmailType.POP3;
            try
            {
                EmailHelper.EmailHelper eh = new EmailHelper.EmailHelper(server, int.Parse(port), username, password, isTSL, et);
                int count = eh.GetEmailCount();
                if (!IsChange)
                {
                    eh.SaveConfToFile();
                }
                else
                {
                    EmailHelper.EmailConfig ecc = new EmailHelper.EmailConfig
                    {
                        Url = server,
                        UserName = username,
                        PassWrod = password,
                        SSl = isTSL,
                        Port = int.Parse(port),
                        EmailType = et
                    };
                    eh.ChangeOneConfig(ecbak, ecc);
                }
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsPunctuation(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (Char.IsPunctuation(e.KeyChar))
            {
                if (e.KeyChar == '.')
                {
                    if (((TextBox)sender).Text.LastIndexOf('.') != -1)
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void AddEmail_Load(object sender, EventArgs e)
        {
            if (!radioButton1.Checked && !radioButton2.Checked)
                radioButton1.Checked = true;
        }

        private void RadioButton2_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = true;
        }

        private void RadioButton1_Click(object sender, EventArgs e)
        {
            radioButton2.Checked = false;
            radioButton1.Checked = true;
        }
    }
}
