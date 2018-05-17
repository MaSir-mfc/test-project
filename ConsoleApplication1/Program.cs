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

            string bucket = "mybucket";
            string key = "我的文件";
            string ak = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            string sk = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
            DateTime now = DateTime.Now;
            int expirationInSeconds = 1200;

            HttpWebRequest req = WebRequest.Create("http://bj.bcebos.com/" + bucket + "/" + key) as HttpWebRequest;
            Uri uri = req.RequestUri;
            req.Method = "GET";

            string signDate = now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");
            Console.WriteLine(signDate);
            string authString = "bce-auth-v1/" + ak + "/" + signDate + "/" + expirationInSeconds;
            string signingKey = Hex(new HMACSHA256(Encoding.UTF8.GetBytes(sk)).ComputeHash(Encoding.UTF8.GetBytes(authString)));
            Console.WriteLine(signingKey);

            string canonicalRequestString = CanonicalRequest(req);
            Console.WriteLine(canonicalRequestString);

            string signature = Hex(new HMACSHA256(Encoding.UTF8.GetBytes(signingKey)).ComputeHash(Encoding.UTF8.GetBytes(canonicalRequestString)));
            string authorization = authString + "/host/" + signature;
            Console.WriteLine(authorization);

            req.Headers.Add("x-bce-date", signDate);
            req.Headers.Add(HttpRequestHeader.Authorization, authorization);

            HttpWebResponse res;
            string message = "";
            try
            {
                res = req.GetResponse() as HttpWebResponse;
            }
            catch (WebException e)
            {
                res = e.Response as HttpWebResponse;
                message = new StreamReader(res.GetResponseStream()).ReadToEnd();
            }
            Console.WriteLine((int)res.StatusCode);
            Console.WriteLine(res.Headers);
            Console.WriteLine(message);
            Console.ReadLine();
            //new BaiduSEMApi().Respose();

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

        static string CanonicalRequest(HttpWebRequest req)
        {
            Uri uri = req.RequestUri;
            StringBuilder canonicalReq = new StringBuilder();
            canonicalReq.Append(req.Method).Append("\n").Append(UriEncode(Uri.UnescapeDataString(uri.AbsolutePath))).Append("\n");

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
