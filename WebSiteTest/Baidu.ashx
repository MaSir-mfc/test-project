<%@ WebHandler Language="C#" Class="Baidu" %>

using System;
using System.Web;

public class Baidu : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        var _form = context.Request.Form;
        context.Response.Write("执行结果：");
        string _result = string.Empty;
        switch (context.Request.QueryString["type"])
        {
            //case "test":
            //    _result = new BaiduSEMApi().Respose();
            //    break;
            case "sem":
                HelperTool.WriteTextLog(_form.ToString());
                HelperTool.WriteTextLog(_form["param"]);
                _result = new SEM().GetResponseData(_form["url"], _form["method"], _form["time"], _form["auth"], System.Text.Encoding.UTF8.GetBytes(_form["param"]));
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