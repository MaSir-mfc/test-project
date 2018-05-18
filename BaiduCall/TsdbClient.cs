/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：TsdbClient
 *  所属项目：
 *  创建用户：马发才
 *  创建时间：2018/5/18 9:43:55
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
using System.Text;
using System.Threading.Tasks;

namespace BaiduCall
{
    public class TsdbClient
    {
        public TsdbClient(long accountId, string url)
        {
            m_url = new Uri(url);
            Init(accountId);
        }

        public TsdbClient(long accountId, Uri url)
        {
            m_url = url;
            Init(accountId);
        }
        public void Init(long accountId)
        {
            //从数据库读取配置信息
            m_ak = "5fc0bc28cb9e47a18930b4a052e4250c";
            m_sk = "34534f37bb024bd3876e27703959ce0b1";

            m_timeStr = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");
            m_auth = HelperTool.GenAuth(m_ak, m_sk, m_timeStr, m_expirationSeconds, m_url, m_method);
        }
        public const string m_dataApiHost = "https://{0}.tsdb.iot.bj.baidubce.com/{1}";
        public const string m_manageApiHost = "https://tsdb.gz.baidubce.com/{0}";

        private string m_ak, m_sk, m_method, m_auth, m_timeStr
             , m_expirationSeconds = "1800";
        private Uri m_url;

        public string Respose(JObject param)
        {
            return HelperTool.GetResponseData(m_url, m_method, m_timeStr, m_auth, Encoding.UTF8.GetBytes(param.ToString()));
        }
    }
}
