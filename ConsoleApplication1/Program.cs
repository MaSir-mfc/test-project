using BaiduCall;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            var _result = new BaiduSEMApi().Respose();
            Console.WriteLine(_result);

            #region 循环注册极光用户
            //Console.WriteLine("输入开始的用户id，回车继续");
            //string _minUserId = Console.ReadLine();
            //Console.WriteLine("输入单次读取用户条数，回车开始读取用户数据");
            //int _pageSize = Convert.ToInt32(Console.ReadLine());
            ////最后一个用户id
            //string _lastUserId = string.Empty;
            //for (int i = 0; i < 10; i++)
            //{
            //    var _dt = Sales.GetUserDb(i, _pageSize, _minUserId);
            //    Console.WriteLine("本次总读取用户{0}条", _dt.Rows.Count);
            //    List<string> _list = new List<string>();
            //    for (int j = 0; j < _dt.Rows.Count; j++)
            //    {
            //        _list.Add(string.Format("t{0}", _dt.Rows[j]["user_id"]));
            //    }
            //    if (_dt.Rows.Count > 0)
            //    {
            //        _lastUserId = _dt.Rows[_dt.Rows.Count - 1]["user_id"].ToString();
            //    }
            //    string _uids = string.Join(",", _list);
            //    //try
            //    //{
            //    //    string _url = string.Format("http://1.t.93390.cn/txooo/salesv2/shop/Ajax/ShopOpenAjax.ajax/RegistJPush?uids={1}", _uids);
            //    //    Masir.MaLogHelper.GetLogger("ReadSalesUserDebug").Debug(string.Format("注册极光：{0}", _uids));
            //    //    var _return = JObject.Parse(new WebClient().DownloadString(_url));
            //    //    if (_return["success"].ToString() == "false")
            //    //    {
            //    //        Masir.MaLogHelper.GetLogger("ReadSalesUserServiceError").Error(string.Format("注册极光服务器返回异常：{0}", _uids));
            //    //        Console.WriteLine("注册极光异常：{0}", _uids);
            //    //        break;
            //    //    }
            //    //    else
            //    //    {
            //    //        Console.WriteLine("注册极光成功：{0}", _uids);
            //    //    }
            //    //}
            //    //catch (Exception ex2)
            //    //{
            //    //    Masir.MaLogHelper.GetLogger("ReadSalesUserError").Error(string.Format("再次注册极光异常：{0}", ex2.Message), ex2);
            //    //    break;
            //    //}
            //}
            //Console.WriteLine("本次大于【{0}】的用户注册极光结束，最后一个用户id【{1}】", _minUserId, _lastUserId);

            #endregion

            Console.ReadKey();
        }

    }
}
