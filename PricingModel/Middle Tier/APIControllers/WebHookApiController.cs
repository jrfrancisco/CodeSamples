using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using bringpro.Web.Domain;
using bringpro.Web.Enums;
using bringpro.Web.Models.Requests;
using bringpro.Web.Models.Requests.Users;
using bringpro.Web.Models.Responses;
using bringpro.Web.Services;
using bringpro.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace bringpro.Web.Controllers.Api
{

    [System.Web.Http.RoutePrefix("api/webhooks")]
    public class WebHookApiController : ApiController
    {
        private object jObj;

        [Dependency]
        public IActivityLogService _ActivityLogService { get; set; }

        [Dependency]
        public IUserCreditsService _CreditsService { get; set; }

        [Dependency]
        public IUserProfileService _UserProfileService { get; set; }

        [Dependency]
        public IBrainTreeService _BrainTreeService { get; set; }

        [Route("{id:int}")]
        [HttpPost]
        public HttpResponseMessage Insert(JobStatus id)
        {
            string json = String.Empty;
            ActivityLogAddRequest add = new ActivityLogAddRequest();
            Job job = new Job();

            Dictionary<string, object> values = new Dictionary<string, object>();

            SuccessResponse response = new SuccessResponse();

            if (HttpContext.Current.Request.InputStream.Length > 0)
            {
                HttpContext.Current.Request.InputStream.Position = 0;
                using (var inputStream = new StreamReader(HttpContext.Current.Request.InputStream))
                {

                    json = inputStream.ReadToEnd();
                }

                jObj = JObject.Parse(json);
                values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                int externalJobId = Convert.ToInt32(values["id"]);


                job = JobsService.GetByExternalJobId(externalJobId);

                add.JobId = job.Id;

                add.TargetValue = (int)id;
                add.RawResponse = Newtonsoft.Json.JsonConvert.SerializeObject(jObj);
                add.ActivityType = ActivityTypeId.BringgTaskStatusUpdated;

                //Need an if to check if the userId is null or not - If it is null, then use the Phone Nunber
                if (job.UserId != null)
                {
                    _ActivityLogService.Insert(job.UserId, add);
                }
                else
                {
                    job.UserId = job.Phone;
                    _ActivityLogService.Insert(job.UserId, add);
                }

                JobsService.UpdateJobStatus(id, externalJobId);

            }

            //Will call the CompleteTrancsaction function once delivery is done

            if (id == JobStatus.BringgDone)
            {

                _BrainTreeService.CompleteTransaction(job, add);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

    }
}
