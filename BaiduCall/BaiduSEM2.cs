/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：BaiduSEM2
 *  所属项目：
 *  创建用户：马发才
 *  创建时间：2018/5/17 9:32:25
 *  
 *  功能描述：
 *          1、
 *          2、
 * 
 *  修改标识：
 *  修改描述：
 *  待 完 善：
 *          1、
----------------------------------------------------------------*/

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BaiduCall
{
    public class BaiduSEM2
    {
        public BaiduSEM2(string access, string secret, string path, string method = "POST")
        {
            m_timeStr = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");
            m_access = access;
            m_secret = secret;
            m_path = path;
            m_url = string.Format("{0}{1}", m_domainUrl, m_path);
            m_method = method.ToUpper();
            m_auth = GenAuth();
        }

        private string m_access, m_secret, m_method, m_url, m_path, m_auth, m_timeStr
            , m_expirationSeconds = "1800", m_signatureHeaders = "host";

        const string m_host = "sem.baidubce.com";
        const string m_domainUrl = "http://sem.baidubce.com/v1/feed/cloud/";

        public JObject BodyData { get; set; }
        public string opUsername { get; set; }
        public string tgUsername { get; set; }
        public string bceUser { get; set; }
        public string opPassword { get; set; }
        public string tgPassword { get; set; }

        public string Response()
        {
            var _header = new JObject();
            _header["opUsername"] = opUsername;
            _header["tgUsername"] = tgUsername;
            _header["bceUser"] = bceUser;
            _header["opPassword"] = opPassword;
            _header["tgPassword"] = tgPassword;

            var _data = new JObject();
            _data["body"] = BodyData;
            _data["header"] = _header;

            return GetResponseData(Encoding.UTF8.GetBytes(_data.ToString()));
        }

        public string GetResponseData(byte[] param)
        {
            var _client = new WebClient();
            _client.Headers.Set("accept-encoding", "gzip, deflate");
            _client.Headers.Set("host", m_host);
            _client.Headers.Set("content-type", "application/json");
            _client.Headers.Set("x-bce-date", m_timeStr);
            _client.Headers.Set(HttpRequestHeader.Authorization, m_auth);
            _client.Headers.Set("accept", "*/*");
            try
            {
                byte[] _responseData = _client.UploadData(m_url, m_method, param);
                return Encoding.UTF8.GetString(_responseData);//解码   
            }
            catch (WebException ex)
            {
                throw ex;
                //var _res = ex.Response as HttpWebResponse;
                //return new StreamReader(_res.GetResponseStream()).ReadToEnd();
            }

            //HttpWebRequest _req = WebRequest.Create(m_url) as HttpWebRequest;
            //_req.Method = m_method;
            //_req.Headers.Set("accept-encoding", "gzip, deflate");
            //_req.Headers.Set("host", m_host);
            //_req.Headers.Set("content-type", "application/json");
            //_req.Headers.Set("x-bce-date", m_timeStr);
            //_req.Headers.Set(HttpRequestHeader.Authorization, m_auth);
            //_req.Headers.Set("accept", "*/*");

            //_req.ContentLength = param.Length;
            //Stream _writer = _req.GetRequestStream();
            //_writer.Write(param, 0, param.Length);
            //_writer.Close();
            //try
            //{
            //    return _req.GetResponse() as HttpWebResponse;
            //}
            //catch (WebException ex)
            //{
            //    return ex.Response as HttpWebResponse;
            //    //message = new StreamReader(_res.GetResponseStream()).ReadToEnd();
            //}
        }

        public string GenAuth()
        {
            string _authString = string.Format("bce-auth-v1/{0}/{1}/{2}", m_access, m_timeStr, m_expirationSeconds);
            string _signingKey = Hex(new HMACSHA256(Encoding.UTF8.GetBytes(m_secret)).ComputeHash(Encoding.UTF8.GetBytes(_authString)));

            string _canonicalRequestString = CanonicalRequest(new Uri(m_url), m_method);
            string _signature = Hex(new HMACSHA256(Encoding.UTF8.GetBytes(_signingKey)).ComputeHash(Encoding.UTF8.GetBytes(_canonicalRequestString)));
            string _authorization = string.Format("{0}/host/{1}", _authString, _signature);
            return _authorization;
        }

        static string UriEncode(string input, bool encodeSlash = false)
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

        static string Hex(byte[] data)
        {
            var sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        static string CanonicalRequest(Uri uri, string method)
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
    }
}
