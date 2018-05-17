<%@ WebHandler Language="C#" Class="Baidu" %>

using System;
using System.Web;
using BaiduCall;

public class Baidu : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
        switch (context.Request.QueryString["type"])
        {
            case "sem":
                new BaiduSEMApi().Respose();
                break;
            default:
                break;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}