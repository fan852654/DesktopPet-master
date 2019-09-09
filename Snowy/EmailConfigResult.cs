using Newtonsoft.Json;
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
    public partial class EmailConfigResult : Form
    {
        private List<EmailHelper.EmailConfig> list;
        public EmailConfigResult()
        {
            InitializeComponent();
        }

        private void EmailConfigResult_Load(object sender, EventArgs e)
        {
            list = EmailHelper.EmailHelper.GetConfigsFormConfig();
            foreach(var ec in list)
            {
                listBox1.Items.Add(ec.ToString());
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            else
            {
                EmailHelper.EmailConfig ec = JsonConvert.DeserializeObject<EmailHelper.EmailConfig>(listBox1.SelectedItem.ToString());
                AddEmail ae = new AddEmail(ec);
                ae.ShowDialog();
                list = EmailHelper.EmailHelper.GetConfigsFormConfig();
                listBox1.Items.Clear();
                foreach (var eeec in list)
                {
                    listBox1.Items.Add(eeec.ToString());
                }
            }
        }
    }
}
