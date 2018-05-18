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
using System.Net;
using System.Text;

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
            m_url = new Uri(string.Format(m_domainUrl, path));
            m_method = method.ToUpper();

            m_auth = HelperTool.GenAuth(m_access, m_secret, m_timeStr, m_expirationSeconds, m_url, m_method);
        }

        private string m_access, m_secret, m_method, m_auth, m_timeStr
            , m_expirationSeconds = "1800";
        private Uri m_url;
        
        const string m_domainUrl = "http://sem.baidubce.com/v1/feed/cloud/{0}";
        const string m_baiduYunUrl = "http://180.76.59.199/baidu.ashx?type=sem";

        public JObject HeaderData;
        public JObject BodyData;
        public string Request()
        {
            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}&", "url", m_url);
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
    }
}
