using FTPHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace DownloadHelper
{
    public class DownloadHelper
    {
        private static List<Thread> Jobs = new List<Thread>();
        private static Dictionary<WebClient, DownloadJobInfo> WC_DJI_Map = new Dictionary<WebClient, DownloadJobInfo>();
        private static Dictionary<DownloadJobInfo, int> DownloadPercent = new Dictionary<DownloadJobInfo, int>();
        private static List<string> tmpData = new List<string>();
        public static void StartHttpDownload(Uri uri,string path,string filename,bool isFugai)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(StartThreadHttp));
            DownloadJobInfo dji = new DownloadJobInfo
            {
                uri = uri,
                path = path,
                filename = filename,
                dt = DownloadType.HTTP,
                isFugai = isFugai
            };
            thread.Name = dji.ToString();
            DownloadPercent.Add(dji, 0);
            Jobs.Add(thread);
            thread.Start(dji);
        }
        #region httpDownload
        private static void StartThreadHttp(object obj)
        {
            DownloadJobInfo dji = (DownloadJobInfo)obj;
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            wc.Proxy = WebRequest.DefaultWebProxy;
            WC_DJI_Map.Add(wc, dji);
            wc.DownloadFileAsync(dji.uri, Path.Combine(dji.path, dji.filename));
            while (WC_DJI_Map.ContainsKey(wc))
            {
                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
            if (Jobs.SingleOrDefault(m => m.Name == dji.ToString())!= null)
            {
                Jobs.Remove(Jobs.SingleOrDefault(m => m.Name == dji.ToString()));
            }
            GC.Collect();
            return;
        }
        private static void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            WC_DJI_Map[(WebClient)sender].Status = "已完成";
            if (WC_DJI_Map.ContainsKey((WebClient)sender))
            {
                WC_DJI_Map.Remove((WebClient)sender);
                GC.Collect();
            }
        }
        private static void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadPercent[WC_DJI_Map[(WebClient)sender]] = e.ProgressPercentage;
            WC_DJI_Map[(WebClient)sender].Status = "下载中..";
        }
        public static int GetFileDownloadPercent(DownloadJobInfo dji)
        {
            return DownloadPercent.SingleOrDefault(m => m.Key.filename == dji.filename && m.Key.path == dji.path && m.Key.uri.ToString() == dji.uri.ToString()).Value;
        }
        public static Dictionary<DownloadJobInfo, int> GetDownloadJobs()
        {
            //Dictionary<DownloadJobInfo, int> dd = new Dictionary<DownloadJobInfo, int>();
            //var lsit = DownloadPercent.Where(m => m.Key.dt == DownloadType.HTTP);
            //foreach(var a in lsit)
            //{
            //    dd.Add(a.Key,a.Value);
            //}
            //return dd;
            return DownloadPercent;
        }
        public static void RemoveAllData()
        {
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs", "remove.tmp");
            if (File.Exists(path1))
            {
                List<string> old = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(path1));
                if(old != null && old.Count > 0)
                {
                    string[] news = old.ToArray();
                    old.Clear();
                    foreach(string str in news)
                    {
                        try
                        {
                            if (File.Exists(str))
                            {
                                File.Delete(str);
                            }
                            old.Remove(str);
                        }
                        catch
                        {
                            old.Add(str);
                        }
                    }
                }
            }
            foreach (Thread t in Jobs)
            {
                t.Abort();
            }
            Jobs.Clear();
            foreach(var t in WC_DJI_Map)
            {
                t.Key.Dispose();
            }
            WC_DJI_Map.Clear();
            foreach(var t in DownloadPercent)
            {
                if(t.Value != 100)
                {
                    string path = Path.Combine(t.Key.path, t.Key.filename);
                    try
                    {
                        File.Delete(path);
                    }
                    catch
                    {
                        tmpData.Add(path);
                    }
                }
            }
            SaveTmpNxtDel();
            DownloadPercent.Clear();
            GC.Collect();
        }
        private static void SaveTmpNxtDel()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs", "remove.tmp");
            if (!File.Exists(path))
                File.Create(path).Close();
            List<string> old = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(path));
            if (old == null)
                old = new List<string>();
            old.AddRange(tmpData);
            File.WriteAllText(path,JsonConvert.SerializeObject(old.Distinct()));
        }
        #endregion
        #region ftpDownload
        public static long GetFTPFileLength(Uri uri,string username,string password)
        {
            FTPHelper.FTPHelper ftp = new FTPHelper.FTPHelper(uri.ToString(), username, password);
            return ftp.GetFileLength();
        }
        public static void StartFTPDownload(string uri,string username,string password,string path,string filename,bool isFugai)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(Ftpdownloadfile));
            if (!isFugai)
            {
                int i = 0;
                string pathfile = Path.Combine(path, filename);
                while (File.Exists(pathfile))
                {
                    filename = "(" + i + ")" + filename;
                    pathfile = Path.Combine(path, filename);
                    i++;
                }
            }
            DownloadJobInfo dji = new DownloadJobInfo
            {
                uri = new Uri(uri),
                path = path,
                filename = filename,
                dt = DownloadType.FTP,
                isFugai = isFugai,
                username = username,
                password = password,
                Length = GetFTPFileLength(new Uri(uri),username,password)
            };
            thread.Name = dji.ToString();
            DownloadPercent.Add(dji, 0);
            Jobs.Add(thread);
            thread.Start(dji);
        }
        private static void Ftpdownloadfile(object djiobj)
        {
            long filelength = 0;
            DownloadJobInfo dji = (DownloadJobInfo)djiobj;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dji.uri);
            filelength = dji.Length;
            try
            {
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(dji.username, dji.password);
                dji.Status = "下载中...";
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (FileStream fs = new FileStream(Path.Combine(dji.path, dji.filename), FileMode.OpenOrCreate))
                        {
                            byte[] buffer = new byte[102400];
                            int read = 0;
                            int allready = 0;
                            do
                            {
                                read = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, read);
                                fs.Flush();
                                allready += read;
                                DownloadPercent[dji] = int.Parse((long.Parse(allready.ToString()) / filelength * 100).ToString());
                            } while (!(read == 0));
                            fs.Flush();
                            fs.Close();
                        }
                    }
                }
                dji.Status = "已完成！";
            }
            catch(Exception ex)
            {
                dji.Status = "出现错误：" + ex.Message;
            }
        }
        public static Dictionary<DownloadJobInfo, int> GetDownloadJobs_FTP()
        {
            var list = DownloadPercent.Where(m => m.Key.dt == DownloadType.FTP);
            Dictionary<DownloadJobInfo, int> dic = new Dictionary<DownloadJobInfo, int>();
            foreach (var item in list)
            {
                dic.Add(item.Key, item.Value);
            }
            return dic;
        }
        #endregion
    }
    public enum DownloadType
    {
        HTTP=1,
        FTP=2
    }
    public class DownloadJobInfo
    {
        public Uri uri { get; set; }
        public string path { get; set; }
        public string filename { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool isFugai { get; set; }
        public string Status { get; set; }
        public DownloadType dt { get; set; }
        public long Length { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string GetMD5()
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(copyNew(this).ToString()));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public DownloadJobInfo copyNew(DownloadJobInfo ddd)
        {
            DownloadJobInfo dji = new DownloadJobInfo
            {
                uri = ddd.uri,
                path = ddd.path,
                filename = ddd.filename,
                username = ddd.username,
                password = ddd.password,
                isFugai = ddd.isFugai,
                dt = ddd.dt
            };
            return dji;
        }
    }
}
