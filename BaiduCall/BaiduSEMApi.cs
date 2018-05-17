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
                string access = "5fc0bc28cb9e47a18930b4a052e4250c",
                    secret = "34534f37bb024bd3876e27703959ce0b",
                    path = "/v1/feed/cloud/AccountFeedService/getAccountFeed";
                BaiduSEM _sem = new BaiduSEM(access, secret, path);
                _sem.opUsername = "baidu-天下商机2140059-1219";
                _sem.opPassword = "Txsj2015617";
                _sem.tgUsername = "baidu-天下商机2140059-1219";
                _sem.tgPassword = "Txsj2015617";
                _sem.bceUser = "9b01628f6b83404c944e0d648da1c667";// "3b4545ae2703599d325b26fdcfd61d99";
                var _json = new JObject();
                _json["accountFeedFields"] = JArray.FromObject(new string[] { "userId", "balance", "budget", "balancePackage", "userStat" });
                _sem.BodyData = _json;
                return _sem.Response();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
