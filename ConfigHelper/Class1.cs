using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigHelper
{
    public class ConfigHelper
    {
        private FileInfo fi { get; set; }
        private Dictionary<string,object> Configs { get; set; }
        public ConfigHelper()
        {
            fi = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsetting.json"));
            Configs = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(fi.FullName));
        }
        public T GetConfig<T>(string key)
        {
            if (Configs.ContainsKey(key))
            {
                T obj = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Configs[key]));
                return obj;
            }
            else
            {
                throw new Exception("Can not Change Type to the type you need");
            }
        }
        public object GetConfig(string key)
        {
            if (Configs.ContainsKey(key))
            {
                return Configs[key]; 
            }
            else
            {
                throw new Exception("Can not find the key in config");
            }
        }
        public object GetConfig(string key,bool nulReturnEx)
        {
            if (nulReturnEx)
            {
                if (Configs.ContainsKey(key))
                {
                    return Configs[key];
                }
                else
                {
                    throw new Exception("Can not find the key in config");
                }
            }
            else
            {
                if (Configs.ContainsKey(key))
                {
                    return Configs[key];
                }
                else
                {
                    return null;
                }
            }
        }
        public void AddConfig(KeyValuePair<string,object> kvp)
        {
            Configs.Add(kvp.Key,kvp.Value);
            File.WriteAllText(fi.FullName, JsonConvert.SerializeObject(Configs));
        }
        public void SetConfig(KeyValuePair<string,object> keyValuePair)
        {
            Configs[keyValuePair.Key] = keyValuePair.Value;
        }
        public void SaveConfig()
        {
            File.WriteAllText(fi.FullName, JsonConvert.SerializeObject(Configs));
        }
    }
}
