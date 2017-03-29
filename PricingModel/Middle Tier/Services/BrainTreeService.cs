using Braintree;
using Microsoft.Practices.Unity;
using bringpro.Web.Domain;
using bringpro.Web.Enums;
using bringpro.Web.Models.Requests;
using bringpro.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;


namespace bringpro.Web.Services
{
    public class BrainTreeService : BaseService, IBrainTreeService
    {

        [Dependency]
        public ITransactionLogService _TransactionLogService { get; set; }
        [Dependency]
        public IUserProfileService _UserProfileService { get; set; }
        [Dependency]
        public IActivityLogService _ActivityLogService { get; set; }
        [Dependency]
        public IUserCreditsService _CreditsService { get; set; }


        private DateTime _timeCompleted;
        private DateTime _timeCreated;
        private double _webPrice;
        private double _totalPrice;
        private double _minJobDuration;
        private double _basePrice;

        public bool CompleteTransaction(Job job, ActivityLogAddRequest add)
        {
            bool success = false; 

            List<ActivityLog> list = _ActivityLogService.GetByJobId(add.JobId);
            foreach (var activity in list)
            {

                int currentStatus = activity.TargetValue;

                if (currentStatus == (int)JobStatus.BringgAccepted)
                {
                    _timeCreated = activity.IdCreated;
                }
                if (currentStatus == (int)JobStatus.BringgDone)
                {
                    _timeCompleted = activity.IdCreated;
                }
            }
            TimeSpan timeDifference = _timeCompleted.Subtract(_timeCreated);
            double toMinutes = timeDifference.TotalMinutes;

            CreditCardService cardService = new CreditCardService();
            PaymentRequest payment = new PaymentRequest();
            BrainTreeService brainTreeService = new BrainTreeService();

            List<string> Slugs = new List<string>();

            Slugs.Add("base-price");
            Slugs.Add("price-per-minute");
            Slugs.Add("minimum-job-duration");
            Slugs.Add("website-pricing-model");

            Dictionary<string, WebsiteSettings> dict = WebsiteSettingsServices.getWebsiteSettingsDictionaryBySlug(job.WebsiteId, Slugs);

            WebsiteSettings pricingModel = (dict["website-pricing-model"]);
            WebsiteSettings basePrice = (dict["base-price"]);
            WebsiteSettings pricePerMin = (dict["price-per-minute"]);
            WebsiteSettings jobDuration = (dict["minimum-job-duration"]);

            // - Switch statement to calculate service cost depending on the website's pricing model

            int pricingModelValue = Convert.ToInt32(pricingModel.SettingsValue);
            switch (pricingModelValue)
            {
                case 1:
                    _basePrice = Convert.ToDouble(basePrice.SettingsValue);
                    _totalPrice = _basePrice;
                    break;

                case 2:
                    _webPrice = Convert.ToDouble(pricePerMin.SettingsValue);
                    _minJobDuration = Convert.ToDouble(jobDuration.SettingsValue);

                    if (toMinutes <= _minJobDuration)
                    {
                        _totalPrice = _webPrice * _minJobDuration;
                    }
                    else
                    {
                        _totalPrice = _webPrice * toMinutes;
                    }

                    break;

                case 3:
                    _webPrice = Convert.ToDouble(pricePerMin.SettingsValue);
                    _basePrice = Convert.ToDouble(basePrice.SettingsValue);
                    _totalPrice = _webPrice + _basePrice;
                    break;
            }


            JobsService.UpdateJobPrice(add.JobId, _totalPrice);

            if (job.UserId != null)
            {
                payment.UserId = job.UserId;
            }
            else
            {
                payment.UserId = job.Phone;
            }


            payment.ExternalCardIdNonce = job.PaymentNonce;
            payment.ItemCost = (decimal)_totalPrice;

            //this is where to adjust the total price with coupon.... for the future


            //do the referral for user A.
            // call one service: to look up tokenhash for user b from the user profile table if they have a tokenhash, then
            //they were a referred user. (user b). use an if statement to make sure the tokenhash is not coming in as null

            String TokenHash = _UserProfileService.GetTokenHashByUserId(job.UserId);

            Guid TokenGuid;
            Guid.TryParse(TokenHash, out TokenGuid);



            if (TokenHash != null)
            {
                bool TokenUsed = TokenService.isTokenUsedReferral(TokenHash);

                //then find the user who referred userB
                //second service: get the userId of userA by using the tokenhash of userB
                //take User B's email and find the user id of the person who referred them (userA) and 
                //use the [dbo].[Token_SelectByUserIdAndTokenType] and take UserId from the stored proc

                //[dbo].[Token_GetByGuid]

                //NOTE: User A is the initial friend who referred User B.

                Token GetUserA = TokenService.userGetByGuid(TokenGuid);

                string UserAId = GetUserA.UserId;
                int CouponReferral = GetUserA.TokenType;
                TokenType referral = (TokenType)CouponReferral; //parsing the int into an enum

                if (UserAId != null && referral == TokenType.Invite && TokenUsed == false) //if this user was referred from a friend && that referral coupon type  is 3 && if that coupon is not used, do the thing
                {

                    //give User A a credit of 25 dollars
                    CouponsDomain userCoupon = TokenService.GetReferralTokenByGuid(TokenHash);

                    UserCreditsRequest insertUserACredits = new UserCreditsRequest();
                    insertUserACredits.Amount = userCoupon.CouponValue;
                    insertUserACredits.TransactionType = "Add";
                    insertUserACredits.UserId = UserAId;
                    //_CreditsService.InsertUserCredits(insertUserACredits); // get int value for it and plug it ino the targetValue in activitylogrequest
                    int forTargetValue = _CreditsService.InsertUserCredits(insertUserACredits);


                    //then update the activity log for USER A to tell them that their friend completed their first order and that they were rewarded credits
                    ActivityLogAddRequest addCreditFriend = new ActivityLogAddRequest();

                    addCreditFriend.ActivityType = ActivityTypeId.CreditsFriend;
                    addCreditFriend.JobId = job.Id;
                    addCreditFriend.TargetValue = forTargetValue;
                    addCreditFriend.RawResponse = Newtonsoft.Json.JsonConvert.SerializeObject(insertUserACredits);
                    _ActivityLogService.Insert(UserAId, addCreditFriend);

                    //update user B's activity log to show that they used the credits for their first payment
                    ActivityLogAddRequest addCredit = new ActivityLogAddRequest();

                    addCredit.ActivityType = ActivityTypeId.Credits;
                    addCredit.JobId = job.Id;
                    addCredit.TargetValue = forTargetValue;
                    addCredit.RawResponse = Newtonsoft.Json.JsonConvert.SerializeObject(insertUserACredits);
                    _ActivityLogService.Insert(UserAId, addCredit);
                }
            }

            AdminPaymentService(payment, job.Id);

           return success;
        }

    }
}