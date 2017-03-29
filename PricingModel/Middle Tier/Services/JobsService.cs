using Microsoft.Practices.Unity;
using bringpro.Data;
using bringpro.Web.Classes.Tasks.Bringg.Interfaces;
using bringpro.Web.Domain;
using bringpro.Web.Enums;
using bringpro.Web.Models.Requests;
using bringpro.Web.Models.Responses;
using bringpro.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace bringpro.Web.Services
{
    
    public class JobsService : BaseService, IJobsService

    {

        public static bool UpdateJobPrice(int JobId, double price)
        {
            bool success = false;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Jobs_UpdatePrice"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", JobId);
                    paramCollection.AddWithValue("@Price", price);


                }, returnParameters: delegate (SqlParameterCollection param)
                {
                    success = true;
                });


            return success;

        }

    }

}