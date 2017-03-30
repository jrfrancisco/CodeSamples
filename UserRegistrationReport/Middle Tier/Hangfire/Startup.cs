using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using bringpro.Web.Classes.Filter.HangFire;
using System.Web.Configuration;

// ********** FYI: System Generated File

[assembly: OwinStartupAttribute(typeof(bringpro.Web.Startup))]
namespace bringpro.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
                .UseUnityActivator(UnityConfig.GetContainer());

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangFireAuthorizationFilter()}
            });

            //SignalR

   

            app.MapSignalR();

        }


  

    }
}
