using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Snowy
{
    public class EatThread
    {
        private ConfigHelper.ConfigHelper configHelper { get; set; }
        public static string[] EatFileAllowType;
        public static string[] EatFileNotAllowType;
        public static DirectoryInfo EatPath;
        public static int EatBetween = 0;
        private System.Windows.Forms.NotifyIcon ni;
        public Action SetMap { get; set; }
        public EatThread(ConfigHelper.ConfigHelper configHelper,Action SetMap, System.Windows.Forms.NotifyIcon notifyIcon1)
        {
            this.configHelper = configHelper;
            EatFileAllowType = configHelper.GetConfig("EatFileAllowType").ToString().Split(',');
            EatFileNotAllowType = configHelper.GetConfig("EatFileNotAllowType").ToString().Split(',');
            EatPath = new DirectoryInfo(configHelper.GetConfig("EatPath").ToString());
            EatBetween = int.Parse(configHelper.GetConfig("EatBetween").ToString());
            this.SetMap = SetMap;
            ni = notifyIcon1;
        }
        public void Start()
        {
            if (configHelper.GetConfig("EatFileEnable").ToString().Equals("true"))
            {
                Task task = new Task(EatAction);
                task.Start();
            }
        }
        public void EatAction()
        {
            while (true)
            {
                SetMap();
                if (!EatPath.Exists)
                    return;
                FileInfo[] filist = EatPath.EnumerateFiles().ToArray();
                List<FileInfo> caneat = new List<FileInfo>();
                foreach(FileInfo f in filist)
                {
                    if(!f.Extension.Trim().Equals("") && EatFileAllowType.Contains(f.Extension.Substring(1,f.Extension.Length - 1)) &&  !EatFileNotAllowType.Contains(f.Extension.Substring(1, f.Extension.Length - 1)))
                    {
                        caneat.Add(f);
                    }
                }
                if(caneat.Count != 0)
                {
                    Random r = new Random();
                    int index = r.Next(0, caneat.Count - 1);
                    ni.ShowBalloonTip(5, "文件被吃", caneat[index].Name + "已经被喂食！！", System.Windows.Forms.ToolTipIcon.Info);
                    caneat[index].Delete();
                    caneat.Clear();
                }
                Thread.Sleep(EatBetween);
            }
        }
    }
}
