<%@ WebHandler Language="C#" Class="Baidu" %>

using System;
using System.Web;
using System.Net;

public class Baidu : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        var _form = context.Request.Form;
        string _result = string.Empty;
        switch (context.Request.QueryString["type"])
        {
            case "sem":
                if (_form != null && !string.IsNullOrWhiteSpace(_form.ToString()))
                {
                    try
                    {
                        HelperTool.WriteTextLog(_form["url"]);
                        _result = HelperTool.GetResponseData(new Uri(_form["url"]), _form["method"], _form["time"]
                            , _form["auth"], System.Text.Encoding.UTF8.GetBytes(_form["param"]));
                    }
                    catch (WebException ex)
                    {
                        var _res = ex.Response as HttpWebResponse;
                        _result = HelperTool.Json(false, new System.IO.StreamReader(_res.GetResponseStream()).ReadToEnd());
                    }
                }
                else
                {
                    _result = HelperTool.Json(false, "请使用post请求");
                }
                break;
            default:
                break;
        }
        context.Response.Write(_result);
    }



    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}