using Newtonsoft.Json;
using S22.Imap;
using S22.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmailHelper
{
    public enum EmailType
    {
        IMAP=1,
        POP3=2
    }
    public class EmailConfig
    {
        public string UserName { get; set; }
        public string PassWrod { get; set; }
        public bool SSl { get; set; }
        public int Port { get; set; }
        public string Url { get; set; }
        public EmailType EmailType { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string GetMD5()
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(this.ToString()));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
    public class EmailHelper
    {
        private EmailType emailtype { get; set; }
        private ImapClient imapclient { get; set; }
        private Pop3Client popclient { get; set; }
        protected string url { get; set; }
        protected int port { get; set; }
        protected string username { get; set; }
        protected string password { get; set; }
        protected bool ssl { get; set; }
        public static List<EmailConfig> GetConfigsFormConfig()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs", "emailconfig.json");
            if (!File.Exists(path))
                File.Create(path).Close();
            string filedetil = File.ReadAllText(path);
            var lll = JsonConvert.DeserializeObject<List<EmailConfig>>(filedetil);
            if (lll == null)
            {
                return new List<EmailConfig>();
            }
            else
            {
                return lll;
            }
        }
        public EmailHelper(string url,int port,string username,string password,bool ssl,EmailType et)
        {
            emailtype = et;
            this.url = url;
            this.port = port;
            this.username = username;
            this.password = password;
            this.ssl = ssl;
            switch (emailtype)
            {
                case (EmailType.IMAP):
                    imapclient = new ImapClient(url, port, ssl);
                    imapclient.Login(username, password, S22.Imap.AuthMethod.Login);
                    break;
                case (EmailType.POP3):
                    popclient = new Pop3Client(url, port, ssl);
                    popclient.Login(username, password, S22.Pop3.AuthMethod.Login);
                    break;
                default:
                    break;
            }
        }
        public EmailHelper(EmailConfig ec)
        {
            emailtype = ec.EmailType;
            this.url = ec.Url;
            this.port = ec.Port;
            this.username = ec.UserName;
            this.password = ec.PassWrod;
            this.ssl = ec.SSl;
            switch (emailtype)
            {
                case (EmailType.IMAP):
                    imapclient = new ImapClient(url, port, ssl);
                    imapclient.Login(username, password, S22.Imap.AuthMethod.Login);
                    break;
                case (EmailType.POP3):
                    popclient = new Pop3Client(url, port, ssl);
                    popclient.Login(username, password, S22.Pop3.AuthMethod.Login);
                    break;
                default:
                    break;
            }
        }
        public void Disposed()
        {
            try
            {
                if (imapclient != null)
                {
                    imapclient.Logout();
                    imapclient.Dispose();
                }
                if (popclient != null)
                {
                    popclient.Logout();
                    popclient.Dispose();
                }
            }
            catch {; }
        }
        public void SaveConfToFile()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs", "emailconfig.json");
            if (!File.Exists(path))
                File.Create(path).Close();
            string filedetil = File.ReadAllText(path);
            List<EmailConfig> confList = JsonConvert.DeserializeObject<List<EmailConfig>>(filedetil);
            if (confList == null)
                confList = new List<EmailConfig>();
            EmailConfig ec = new EmailConfig
            {
                EmailType = emailtype,
                UserName = username,
                PassWrod = password,
                Port = port,
                SSl = ssl,
                Url = url
            };
            confList.Add(ec);
            File.WriteAllText(path, JsonConvert.SerializeObject(confList));
        }
        public void ChangeOneConfig(EmailConfig old,EmailConfig newc)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs", "emailconfig.json");
            if (!File.Exists(path))
                File.Create(path).Close();
            string filedetil = File.ReadAllText(path);
            List<EmailConfig> confList = JsonConvert.DeserializeObject<List<EmailConfig>>(filedetil);
            if (confList == null)
                confList = new List<EmailConfig>();
            if (confList.Contains(old))
            {
                confList.Remove(old);
                confList.Add(newc);
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(confList));
        }
        public MailMessage[] GetAllEmailMessages()
        {
            MailMessage[] messages;
            switch (emailtype)
            {
                case (EmailType.IMAP):
                    using (imapclient)
                    {
                        IEnumerable<uint> uids = imapclient.Search(SearchCondition.Unseen());
                        messages = imapclient.GetMessages(uids, S22.Imap.FetchOptions.Normal).ToArray();
                    }
                    break;
                case EmailType.POP3:
                    using (popclient)
                    {
                        IEnumerable<uint> uids = popclient.GetMessageNumbers();
                        messages = popclient.GetMessages(uids.ToArray(), S22.Pop3.FetchOptions.Normal).ToArray();
                    }
                    break;
                default:
                    messages = new List<MailMessage>().ToArray();
                    break;
            }
            return messages;
        }
        public int GetEmailCount()
        {
            switch (emailtype)
            {
                case (EmailType.IMAP):
                    using (imapclient)
                    {
                        IEnumerable<uint> uids = imapclient.Search(SearchCondition.Unseen());
                        return uids.Count();
                    }
                case EmailType.POP3:
                    using (popclient)
                    {
                        IEnumerable<uint> uids = popclient.GetMessageNumbers();
                        return uids.Count();
                    }
                default:
                    return 0;
            }
        }
    }
}
