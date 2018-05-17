using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// SEM 的摘要说明
/// </summary>
public class SEM
{
    public SEM()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 请求百度云推广信息流返回
    /// </summary>
    /// <param name="requestUrl">请求地址</param>
    /// <param name="method">请求方式</param>
    /// <param name="timeStr">生成签名的时间</param>
    /// <param name="auth">签名</param>
    /// <param name="param">json参数</param>
    /// <returns></returns>
    public string GetResponseData(string requestUrl, string method, string timeStr, string auth, byte[] param)
    {
        if (string.IsNullOrWhiteSpace(requestUrl)) { return HelperTool.Json(false, "url参数不能空"); }
        if (string.IsNullOrWhiteSpace(method)) { return HelperTool.Json(false, "method"); }
        if (string.IsNullOrWhiteSpace(timeStr)) { return HelperTool.Json(false, "time参数不能空"); }
        if (string.IsNullOrWhiteSpace(auth)) { return HelperTool.Json(false, "auth参数不能空"); }
        
        var _client = new WebClient();
        _client.Headers.Set("accept-encoding", "gzip, deflate");
        _client.Headers.Set("host", new Uri(requestUrl).Host);
        _client.Headers.Set("content-type", "application/json");
        _client.Headers.Set("x-bce-date", timeStr);
        _client.Headers.Set(HttpRequestHeader.Authorization, auth);
        _client.Headers.Set("accept", "*/*");
        try
        {
            byte[] _responseData = _client.UploadData(requestUrl, method, param);
            return System.Text.Encoding.UTF8.GetString(_responseData);//解码   
        }
        catch (WebException ex)
        {
            throw ex;
            //var _res = ex.Response as HttpWebResponse;
            //return new StreamReader(_res.GetResponseStream()).ReadToEnd();
        }
    }
}