using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPHelper
{
    public class FTPHelper
    {
        private string url { get; set; }
        private string path { get; set; }
        private string username { get; set; }
        private string password { get; set; }
        private FtpWebRequest reqFTP { get; set; }
        public FTPHelper(string url, string username, string password, string path = "")
        {
            Uri u = new Uri(url + path);
            path = u.AbsolutePath;
            url = u.Scheme + "://" + u.Host;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url + path));
            reqFTP.Credentials = new NetworkCredential(username, password);
            this.url = url;
            this.path = path;
            this.username = username;
            this.password = password;
        }
        public List<RouteDetil> GetDirctory()
        {
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url + path));
            reqFTP.Credentials = new NetworkCredential(username, password);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            WebResponse response = reqFTP.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名
            List<RouteDetil> strs = new List<RouteDetil>();
            string line = reader.ReadLine();
            while (line != null)
            {
                if (line.Contains("<DIR>"))
                {
                    RouteDetil rd = new RouteDetil
                    {
                        ChangeTime = line.Substring(0, 17),
                        Name = line.Substring(line.LastIndexOf("<DIR>") + 5).Trim(),
                        Type = RouteType.Dir
                    };
                    strs.Add(rd);
                }
                line = reader.ReadLine();
            }
            reader.Close();
            response.Close();
            return strs;
        }
        public List<RouteDetil> GetFile()
        {
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url + path));
            reqFTP.Credentials = new NetworkCredential(username, password);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            WebResponse response = reqFTP.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名
            List<RouteDetil> strs = new List<RouteDetil>();
            string line = reader.ReadLine();
            while (line != null)
            {
                if (!line.Contains("<DIR>"))
                {
                    RouteDetil rd = new RouteDetil
                    {
                        ChangeTime = line.Substring(0, 17),
                        Length = long.Parse(line.Substring(18, 21).Trim()),
                        Name = line.Substring(39).Trim(),
                        Type = RouteType.File
                    };
                    strs.Add(rd);
                }
                line = reader.ReadLine();
            }
            reader.Close();
            response.Close();
            return strs;
        }
        public long GetFileLength()
        {
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url + path));
            reqFTP.Credentials = new NetworkCredential(username, password);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            WebResponse response = reqFTP.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            List<RouteDetil> strs = new List<RouteDetil>();
            string line = reader.ReadLine();
            while (line != null)
            {
                if (!line.Contains("<DIR>"))
                {
                    RouteDetil rd = new RouteDetil
                    {
                        ChangeTime = line.Substring(0, 17),
                        Length = long.Parse(line.Substring(18, 21).Trim()),
                        Name = line.Substring(39).Trim(),
                        Type = RouteType.File
                    };
                    strs.Add(rd);
                }
                line = reader.ReadLine();
            }
            reader.Close();
            response.Close();
            return strs.First().Length;
        }
        public void SetPath(string path)
        {
            this.path = path;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url +this.path));
            reqFTP.Credentials = new NetworkCredential(username, password);
        }
        public void SetPath(Uri uri)
        {
            path = uri.AbsolutePath;
            url = uri.Scheme + "://" + uri.Host;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
            reqFTP.Credentials = new NetworkCredential(username, password);
        }
        public void SetPwd(string pwd)
        {
            password = pwd;
            reqFTP.Credentials = new NetworkCredential(username, password);
        }
        public void SetUsr(string usr)
        {
            username = usr;
            reqFTP.Credentials = new NetworkCredential(username, password);
        }
        public string GetUrl()
        {
            return url + path;
        }
        public string GetUrlStr()
        {
            return url;
        }
        public string GetPathStr()
        {
            return path;
        }
        public string GetUsr()
        {
            return username;
        }
        public string GetPwd()
        {
            return password;
        }
    }
    public enum RouteType
    {
        File =0,
        Dir = 1
    }
    public class RouteDetil : IComparable
    {
        public string Name { get; set; }
        public RouteType Type { get; set; }
        public long Length { get; set; }
        public string ChangeTime { get; set; }
        public int CompareTo(object obj)
        {
            if (obj is RouteDetil)
            {
                return Length.CompareTo(((RouteDetil)obj).Length);
            }
            return 1;
        }
    }
    public class RouteDetilComparer : IComparer<RouteDetil>
    {
        public int Compare(RouteDetil x, RouteDetil y)
        {
            return x.Length.CompareTo(y.Length);
        }
    }
}
