using Hangfire;
using Sabio.Web.Classes.Tasks.HangFire;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Hangfire.SqlServer;
using System.Web.Configuration;

namespace Sabio.Web
{
    public class MvcApplication : System.Web.HttpApplication 
    {

        protected void Application_Start()
        {
           
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        
            //  enable Unity injection - based on example from http://www.asp.net/web-api/overview/advanced/dependency-injection
            UnityConfig.RegisterComponents(System.Web.Http.GlobalConfiguration.Configuration);
            //UnityConfig.RegisterComponents();

            if (Environment.GetEnvironmentVariable("bringproProduction") == "true")
            {
                HangfireBootstrapper.Instance.Start();

                RecurringJob.AddOrUpdate<UserRegistrationReport>("UserReports", task => task.InsertRegistrationReport(), "0 3 * * 0-6", TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));

                RecurringJob.AddOrUpdate<EmailCampaignsService>("MailChimp Users", task => task.InsertUsersToList(), "0 3 * * 0-6", TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));



            }

        }

        protected void Application_End(object sender, EventArgs e)
        {
            HangfireBootstrapper.Instance.Stop();
        }



    }
}
