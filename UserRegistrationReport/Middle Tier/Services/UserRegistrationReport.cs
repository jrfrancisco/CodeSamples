using Microsoft.Practices.Unity;
using Sabio.Data;
using Sabio.Web.Classes.Tasks.HangFire;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class UserRegistrationReport : BaseService, IUserRegistrationReport
    {

        [Dependency]
        public IUserProfileService _UserProfileService { get; set; }

        public void InsertRegistrationReport()
        {
            List<UserWebsite> userWebsite = _UserProfileService.GetWebsiteId();

            DateTime startDate = new DateTime(2017, 2, 1);
            DateTime currentDate = DateTime.Now;

            foreach (DateTime day in UtilityService.EachDay(startDate, currentDate))
            {
                foreach (var website in userWebsite)
                {
                    DataProvider.ExecuteNonQuery(GetConnection, "dbo.Reports_UserRegistrationReport_Update"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {

                    paramCollection.AddWithValue("@Date", day);
                    paramCollection.AddWithValue("@WebsiteId", website.WebsiteId);

                });
                }
            }
        }
    }
}



