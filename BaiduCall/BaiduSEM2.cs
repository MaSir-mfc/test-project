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
        public BaiduSEM2(long accountId, string path)
        {
            //从数据库读取配置信息
            string access = "5fc0bc28cb9e47a18930b4a052e4250c",
                    secret = "34534f37bb024bd3876e27703959ce0b";
            Init(access, secret, path);

            var _header = new JObject();
            _header["opUsername"] = "baidu-天下商机2140059-1219";
            _header["opPassword"] = "Txsj2015617";
            _header["tgUsername"] = "baidu-天下商机2140059-1219";
            _header["tgPassword"] = "Txsj2015617";
            _header["bceUser"] = "7a128075d1de45e4b897ae3191e851eb";
            this.HeaderData = _header;
        }

        public void Init(string access, string secret, string path, string method = "POST")
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
            , m_expirationSeconds = "1800";

        const string m_host = "sem.baidubce.com";
        const string m_domainUrl = "http://sem.baidubce.com/v1/feed/cloud/";
        const string m_baiduYunUrl = "http://180.76.59.199/baidu.ashx?type=sem";

        public JObject HeaderData;
        public JObject BodyData;
        public string Request()
        {
            StringBuilder postData = new StringBuilder();
            //postData.AppendFormat("{0}={1}{2}&", "url", m_domainUrl, m_path);
            postData.AppendFormat("{0}={1}&", "method", m_method);
            postData.AppendFormat("{0}={1}&", "time", m_timeStr);
            postData.AppendFormat("{0}={1}&", "auth", m_auth);
            if (HeaderData != null)
            {
                var _data = new JObject();
                if (BodyData != null) _data["body"] = BodyData;
                _data["header"] = HeaderData;
                postData.AppendFormat("{0}={1}", "param", _data.ToString());
            }
            var _client = new WebClient();
            _client.Headers.Set("content-type", "application/x-www-form-urlencoded");
            try
            {
                var _result = _client.UploadData(m_baiduYunUrl, System.Text.Encoding.UTF8.GetBytes(postData.ToString()));
                return Encoding.UTF8.GetString(_result);
            }
            catch (WebException ex)
            {
                throw ex;
                //var _res = ex.Response as HttpWebResponse;
                //return new StreamReader(_res.GetResponseStream()).ReadToEnd();
            }
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
        /// <summary>
        /// 地址字符编码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodeSlash"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 数据转换成十六进制
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static string Hex(byte[] data)
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
