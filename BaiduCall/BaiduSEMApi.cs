/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：BaiduSEMApi
 *  所属项目：
 *  创建用户：马发才
 *  创建时间：2018/5/16 16:51:39
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduCall
{
    public class BaiduSEMApi
    {

        public string Respose()
        {
            try
            {
                string _path = "AccountFeedService/getAccountFeed";
                BaiduSEM2 _sem = new BaiduSEM2(0, _path);
                var _json = new JObject();
                _json["accountFeedFields"] = JArray.FromObject(new string[] { "userId", "balance", "budget", "balancePackage", "userStat" });
                _sem.BodyData = _json;

                return _sem.Request();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
