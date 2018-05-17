using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// HelperTool 的摘要说明
/// </summary>
public class HelperTool
{
    public HelperTool()
    { }

    public static string Json(bool success, object msg, dynamic otherInfo = null)
    {
        JObject _newJson = new JObject();
        _newJson["success"] = success;
        _newJson["msg"] = msg.ToString();
        if (otherInfo != null)
        {
            Type _type = otherInfo.GetType();
            var _prop = _type.GetProperties();
            foreach (var _p in _prop)
            {
                _newJson[_p.Name] = _p.GetValue(otherInfo);
            }
        }
        return _newJson.ToString();
    }

    public static void WriteTextLog(string strMessage)
    {
        DateTime time = DateTime.Now;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"Log\";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".System.txt";
        StringBuilder str = new StringBuilder();
        str.Append("Time:    " + time.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
        str.Append("Message: " + strMessage + "\r\n");
        str.Append("-----------------------------------------------------------\r\n\r\n");
        StreamWriter sw;
        if (!File.Exists(fileFullPath))
        {
            sw = File.CreateText(fileFullPath);
        }
        else
        {
            sw = File.AppendText(fileFullPath);
        }
        sw.WriteLine(str.ToString());
        sw.Close();
    }

}