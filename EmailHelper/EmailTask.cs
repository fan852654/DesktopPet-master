using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmailHelper
{
    public class EmailTask
    {
        private static List<EmailConfig> ConfigList { get; set; }
        private static Dictionary<string,int> User_Count { get; set; }
        private static Dictionary<EmailConfig, string> Config_MD5 { get; set; }
        public static Dictionary<string, List<MailMessage>> MailMessages { get; set; }
        private Task taskThread { get; set; }
        public TimeSpan ScanBetween { get; set; }
        private NotifyIcon NotifyIcon { get; set; }
        public EmailTask(NotifyIcon ni)
        {
            ConfigList = new List<EmailConfig>();
            ScanBetween = new TimeSpan(0, 0, 15);
            User_Count = new Dictionary<string, int>();
            Config_MD5 = new Dictionary<EmailConfig, string>();
            MailMessages = new Dictionary<string, List<MailMessage>>();
            NotifyIcon = ni;
        }
        public void AddNeedScan(EmailConfig ec)
        {
            ConfigList.Add(ec);
        }
        public void Start()
        {
            if (ConfigList != null)
                ConfigList.Clear();
            GC.Collect();
            ConfigList = EmailHelper.GetConfigsFormConfig();
            taskThread = new Task(ThreadOfEmail);
            taskThread.Start();
        }
        public void Stop()
        {
            if(taskThread.Status == TaskStatus.Running)
            {
                taskThread.Dispose();
            }
        }
        public static List<MailMessage> GetAllMailMessage(EmailConfig ec)
        {
            foreach(var ce in Config_MD5)
            {
                if(ce.Key.Url.Equals(ec.Url) && ce.Key.Port.Equals(ec.Port) &&
                    ce.Key.UserName.Equals(ec.UserName) && ce.Key.PassWrod.Equals(ec.PassWrod) && 
                    ce.Key.SSl.Equals(ec.SSl) && ce.Key.EmailType.Equals(ec.EmailType))
                {
                    if (MailMessages.ContainsKey(ce.Value))
                    {
                        return MailMessages[ce.Value];
                    }
                    break;
                }
            }
            return new List<MailMessage>();
        }
        public void ThreadOfEmail()
        {
            while (true)
            {
                if(ConfigList == null)
                {
                    Thread.Sleep(ScanBetween);
                    continue;
                }
                List<EmailConfig> eclist = new List<EmailConfig>(ConfigList);
                foreach (EmailConfig e in eclist)
                {
                    try
                    {
                        EmailHelper eh = new EmailHelper(e);
                        int count = eh.GetEmailCount();
                        eh.Disposed();
                        if (!Config_MD5.ContainsKey(e))
                        {
                            Config_MD5.Add(e, e.GetMD5());
                        }
                        if (!User_Count.ContainsKey(Config_MD5[e]))
                        {
                            User_Count.Add(Config_MD5[e], count);
                        }
                        else
                        {
                            #region 失效
                            //if (User_Count[Config_MD5[e]] != count)
                            //{
                            //    if (User_Count[Config_MD5[e]] > count)
                            //    {
                            //        NotifyIcon.ShowBalloonTip(5, "邮件被删除", e.Url + ":" + e.UserName + "删除了" + (count) + "条邮件", ToolTipIcon.Info);
                            //    }
                            //    else
                            //    {
                            //        NotifyIcon.ShowBalloonTip(5, "收到了新邮件", e.Url + ":" + e.UserName + "收到了" + (count - User_Count[Config_MD5[e]]) + "条新邮件", ToolTipIcon.Info);
                            //    }
                            //    User_Count[Config_MD5[e]] = User_Count[Config_MD5[e]] + count;
                            //}
                            #endregion
                        }
                        if (count != 0)
                        {
                            User_Count[Config_MD5[e]] = User_Count[Config_MD5[e]] + count;
                            NotifyIcon.ShowBalloonTip(5, "收到了新邮件", e.Url + ":" + e.UserName + "收到了" + (count) + "条新邮件", ToolTipIcon.Info);
                            if (!MailMessages.ContainsKey(Config_MD5[e]))
                            {
                                MailMessages.Add(Config_MD5[e], new List<MailMessage>());
                            }
                            EmailHelper ehh = new EmailHelper(e);
                            var ll = ehh.GetAllEmailMessages();
                            MailMessages[Config_MD5[e]].AddRange(ll);
                            ehh.Disposed();
                        }
                        else {; }
                    }
                    catch
                    {
                        ConfigList.Remove(e);
                        Config_MD5.Remove(e);
                    }
                }
                Thread.Sleep(ScanBetween);
            }
        }
    }
}
