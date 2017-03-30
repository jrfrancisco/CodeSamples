using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire.Annotations;
using System.Net;

namespace bringpro.Web.Classes.Filter.HangFire
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        
        public bool Authorize([NotNull] DashboardContext context)
        {
            if (HttpContext.Current.User.IsInRole("Administrator"))
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }

            return false;

        }
    }
}

