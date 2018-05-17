/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：BaiduSEM
 *  所属项目：
 *  创建用户：马发才
 *  创建时间：2018/5/16 16:48:59
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

namespace BaiduCall
{
    public class BaiduSEM
    {
        public BaiduSEM(string access, string secret, string path, string method = "POST")
        {
            m_timeStr = DateTime.Now.ToString("yyyy-mm-dd H:M:s");
            m_access = access;
            m_secret = secret;
            m_path = path;
            m_url = string.Format("{0}{1}", m_host, m_path);
            m_method = method.ToUpper();
        }

        private string m_access, m_secret, m_method, m_url, m_path, m_auth, m_timeStr
            , m_version = "1", m_expirationSeconds = "1800", m_signatureHeaders = "host";

        const string m_host = "sem.baidubce.com";


        const string AddCreativeFeed = "/v1/feed/cloud/CreativeFeedService/addCreativeFeed";

        public JObject BodyData { get; set; }
        public string opUsername { get; set; }
        public string tgUsername { get; set; }
        public string bceUser { get; set; }
        public string opPassword { get; set; }
        public string tgPassword { get; set; }
        public string Response()
        {
            m_auth = GenAuth();
            var _header = new JObject();
            _header["opUsername"] = opUsername;
            _header["tgUsername"] = tgUsername;
            _header["bceUser"] = bceUser;
            _header["opPassword"] = opPassword;
            _header["tgPassword"] = tgPassword;

            var _data = new JObject();
            _data["body"] = BodyData;
            _data["header"] = _header;
            return GetResponseData(_data.ToString());
        }
        /// <summary>  
        /// 返回JSon数据  
        /// </summary>  
        /// <param name="JSONData">要处理的JSON数据</param>  
        /// <returns>返回的JSON处理字符串</returns>  
        public string GetResponseData(string JSONData)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
                //声明一个HttpWebRequest请求  
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}{1}", m_host, m_path));
                request.Method = m_method;
                request.ContentLength = bytes.Length;
                request.ContentType = "text/xml";
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);

                request.Timeout = 90000;
                //设置连接超时时间  
                request.Headers.Set("Pragma", "no-cache");
                request.Headers.Set("accept-encoding", "gzip, deflate");
                request.Headers.Set("host", m_host);
                request.Headers.Set("content-type", "application/json");
                request.Headers.Set("x-bce-date", m_timeStr);
                request.Headers.Set("authorization", m_auth);
                request.Headers.Set("accept", "*/*");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.UTF8;

                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                string strResult = streamReader.ReadToEnd();
                streamReceive.Dispose();
                streamReader.Dispose();

                return strResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenAuth()
        {
            try
            {
                string _val = string.Format("bce-auth-v{0}{1}{2}{3}", m_version, m_access, m_timeStr, m_expirationSeconds);
                var _signingKey = HMACSHA256Encrypt(m_secret, _val);

                string _canonicalRequest = string.Format("{0}\n{1}\n\nhost:{2}", m_method, m_path, m_host);
                var _signature = HMACSHA256Encrypt(_signingKey, _canonicalRequest);
                return string.Format("bce-auth-v{0}{1}{2}{3}{4}{5}", m_version, m_access, m_timeStr, m_expirationSeconds, m_signatureHeaders, _signature);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string HMACSHA256Encrypt(string encryptKey, string encryptText)
        {
            var _reulst = HMACSHA256Encrypt(Encoding.UTF8.GetBytes(encryptKey), Encoding.UTF8.GetBytes(encryptText));
            return Convert.ToBase64String(_reulst);
        }
        public static byte[] HMACSHA256Encrypt(byte[] encryptKey, byte[] encryptText)
        {
            HMACSHA256 _hmac = new HMACSHA256(encryptKey);
            return _hmac.ComputeHash(encryptText);
        }
    }
}
