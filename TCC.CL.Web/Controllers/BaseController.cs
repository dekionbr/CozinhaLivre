using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC.CL.Core.Business;
using TCC.CL.Core.Entities;

namespace TCC.CL.Web.Controllers
{
    [HandleError()]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (TCC.CL.Core.Session.IsClosed())
            {
                TCC.CL.Core.Session.OpenSession();
            }

            var user = User != null ? User.Identity.Name : Request.IsAuthenticated ? Request.LogonUserIdentity.Name : null;
            Acesso ass = new Acesso(user);
            ass.IP = Request.UserHostAddress; //Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ass.IP))
                ass.IP = Request.UserHostName; //.ServerVariables["REMOTE_ADDR"];

            ass.Pagina = Request.RawUrl; //Request.ServerVariables["URL"];

            if (string.IsNullOrEmpty(ass.Pagina))
                ass.Pagina = Request.Url.AbsolutePath; // ServerVariables["SCRIPT_NAME"];

            //System.Web.HttpBrowserCapabilities browser = Request.Browser;

            ass.Navegador = Request.Browser.Browser; //Request.ServerVariables["HTTP_USER_AGENT"];
            ass.Origem = Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : null;


            if (!ass.IP.Contains("::1") && !ass.Pagina.Contains("search"))
                AccessoBussiness.Add(ass);

            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;
            //Log Exception e
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error"

            };
        }
    }
}