using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

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
    /// <summary>
    /// 生成鉴权
    /// </summary>
    /// <param name="ak"></param>
    /// <param name="sk"></param>
    /// <param name="timeStr"></param>
    /// <param name="expirationSeconds"></param>
    /// <param name="url"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    public static string GenAuth(string ak, string sk, string timeStr, string expirationSeconds, Uri url, string method)
    {
        string _authString = string.Format("bce-auth-v1/{0}/{1}/{2}", ak, timeStr, expirationSeconds);
        string _signingKey = Hex(new HMACSHA256(Encoding.UTF8.GetBytes(sk)).ComputeHash(Encoding.UTF8.GetBytes(_authString)));

        string _canonicalRequestString = CanonicalRequest(url, method);
        string _signature = Hex(new HMACSHA256(Encoding.UTF8.GetBytes(_signingKey)).ComputeHash(Encoding.UTF8.GetBytes(_canonicalRequestString)));
        string _authorization = string.Format("{0}/host/{1}", _authString, _signature);
        return _authorization;
    }

    /// <summary>
    /// 地址字符编码
    /// </summary>
    /// <param name="input"></param>
    /// <param name="encodeSlash"></param>
    /// <returns></returns>
    public static string UriEncode(string input, bool encodeSlash = false)
    {
        StringBuilder builder = new StringBuilder();
        foreach (byte b in Encoding.UTF8.GetBytes(input))
        {
            if ((b >= 'a' && b <= 'z') || (b >= 'A' && b <= 'Z') || (b >= '0' && b <= '9') || b == '_' || b == '-' || b == '~' || b == '.')
            {
                builder.Append((char)b);
            }
            else if (b == '/')
            {
                if (encodeSlash)
                {
                    builder.Append("%2F");
                }
                else
                {
                    builder.Append((char)b);
                }
            }
            else
            {
                builder.Append('%').Append(b.ToString("X2"));
            }
        }
        return builder.ToString();
    }

    /// <summary>
    /// 数据转换成十六进制
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string Hex(byte[] data)
    {
        var sb = new StringBuilder();
        foreach (var b in data)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
    /// <summary>
    /// 组建url请求地址
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    public static string CanonicalRequest(Uri uri, string method)
    {
        StringBuilder canonicalReq = new StringBuilder();
        canonicalReq.Append(method).Append("\n").Append(UriEncode(Uri.UnescapeDataString(uri.AbsolutePath))).Append("\n");

        var parameters = HttpUtility.ParseQueryString(uri.Query);
        List<string> parameterStrings = new List<string>();
        foreach (KeyValuePair<string, string> entry in parameters)
        {
            parameterStrings.Add(UriEncode(entry.Key) + '=' + UriEncode(entry.Value));
        }
        parameterStrings.Sort();
        canonicalReq.Append(string.Join("&", parameterStrings.ToArray())).Append("\n");

        string host = uri.Host;
        if (!(uri.Scheme == "https" && uri.Port == 443) && !(uri.Scheme == "http" && uri.Port == 80))
        {
            host += ":" + uri.Port;
        }
        canonicalReq.Append("host:" + UriEncode(host));
        return canonicalReq.ToString();
    }
    /// <summary>
    /// 获得相应数据
    /// </summary>
    /// <param name="requestUrl"></param>
    /// <param name="method"></param>
    /// <param name="timeStr"></param>
    /// <param name="auth"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static string GetResponseData(Uri url, string method, string timeStr, string auth, byte[] param)
    {
        if (url == null) { return HelperTool.Json(false, "url参数不能空"); }
        if (string.IsNullOrWhiteSpace(method)) { return HelperTool.Json(false, "method"); }
        if (string.IsNullOrWhiteSpace(timeStr)) { return HelperTool.Json(false, "time参数不能空"); }
        if (string.IsNullOrWhiteSpace(auth)) { return HelperTool.Json(false, "auth参数不能空"); }

        var _client = new WebClient();
        _client.Headers.Set("accept-encoding", "gzip, deflate");
        _client.Headers.Set("host", url.Host);
        _client.Headers.Set("content-type", "application/json; charset=utf-8");
        _client.Headers.Set("x-bce-date", timeStr);
        _client.Headers.Set(HttpRequestHeader.Authorization, auth);
        _client.Headers.Set("accept", "*/*");

        try
        {
            byte[] _responseData = _client.UploadData(url, method, param);
            return System.Text.Encoding.UTF8.GetString(_responseData);//解码   
        }
        catch (WebException ex)
        {
            throw ex;
        }
    }


    public static void WriteTextLog(string strMessage)
    {
        DateTime time = DateTime.Now;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"Log\";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string fileFullPath = path + time.ToString("yyyy-MM-dd") + ".System.txt";
        StringBuilder str = new StringBuilder();
        str.Append(time.ToString("yyyy-MM-dd HH:mm:ss") + "【");
        str.Append(strMessage + "】\r\n");
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