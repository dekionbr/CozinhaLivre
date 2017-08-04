using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TCC.CL.Core.Business;
using TCC.CL.Core.Entities;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace TCC.CL.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static ISessionFactory _sessionFactory { get; set; }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = TCC.CL.Core.Session.OpenSession().SessionFactory;
                }
                return _sessionFactory;
            }
            set
            {
                _sessionFactory = value;
            }
        }
        protected void Application_Start()
        {
            //NHibernate.Cfg.Environment.UseReflectionOptimizer = true;
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            if (SessionFactory.IsClosed)
            {
                SessionFactory.OpenSession();
            }
        }       
    }
}