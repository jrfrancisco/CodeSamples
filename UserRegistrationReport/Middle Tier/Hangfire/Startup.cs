using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using Sabio.Web.Classes.Filter.HangFire;
using System.Web.Configuration;

// ********** FYI: System Generated File

[assembly: OwinStartupAttribute(typeof(Sabio.Web.Startup))]
namespace Sabio.Web
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
