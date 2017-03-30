using Microsoft.Practices.Unity;
using bringpro.Data;
using bringpro.Web.Classes.Tasks.Bringg.Interfaces;
using bringpro.Web.Domain;
using bringpro.Web.Enums;
using bringpro.Web.Models.Requests;
using bringpro.Web.Models.Requests.Bringg;
using bringpro.Web.Models.Requests.Users;
using bringpro.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace bringpro.Web.Services
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
