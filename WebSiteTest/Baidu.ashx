<%@ WebHandler Language="C#" Class="Baidu" %>

using System;
using System.Web;
using BaiduCall;

public class Baidu : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Write("执行结果：");
        string _result = string.Empty;
        switch (context.Request.QueryString["type"])
        {
            case "sem":
                _result = new BaiduSEMApi().Respose();
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