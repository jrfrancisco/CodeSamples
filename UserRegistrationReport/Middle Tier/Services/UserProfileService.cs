using Microsoft.Practices.Unity;
using Sabio.Data;
using Sabio.Web.Classes.Tasks.Bringg.Interfaces;
using Sabio.Web.Domain;
using Sabio.Web.Enums;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.Bringg;
using Sabio.Web.Models.Requests.Users;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Web.Services
{
    public class UserProfileService : BaseService, IUserProfileService
    {

        public List<UserWebsite> GetWebsiteId()
        {
            List<UserWebsite> userWebsite = new List<UserWebsite>();
            DataProvider.ExecuteCmd(GetConnection, "dbo.UserWebsite_SelectAll"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {

                }, map: delegate (IDataReader reader, short set)
                {
                    UserWebsite uw = new UserWebsite();

                    int startingIndex = 0;

                    uw.UserId = reader.GetSafeString(startingIndex++);
                    uw.WebsiteId = reader.GetSafeInt32(startingIndex++);

                    userWebsite.Add(uw);
                }

                );

            return userWebsite;

        }

        
    }


}
