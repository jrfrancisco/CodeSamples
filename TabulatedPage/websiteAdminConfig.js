(function () {
    "use strict";

    angular.module(APPNAME)
        .config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {

            $routeProvider.when('/website/:Slug/dashboard',
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteDashboard.html',
                controller: 'websiteDashboardController',
                controllerAs: 'webDash'
            }).when('/website/:Slug/jobs',
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteJobs.html',
                controller: 'websiteJobsController',
                controllerAs: 'webJobs'
            }).when('/website/:Slug/jobs/form',
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteJobsForm.html',
                controller: 'webJobsFormController',
                controllerAs: 'webJobsForm'
            }).when('/website/:Slug/jobs/form/:id',
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteJobsForm.html',
                controller: 'webJobsFormController',
                controllerAs: 'webJobsForm'
            }).when('/website/:Slug/users',
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteUsers.html',
                controller: 'websiteUsersController',
                controllerAs: 'webUsers'
            }).when('/website/:Slug/pricing',
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websitePricing.html',
                controller: 'websitePricingController',
                controllerAs: 'webPricing'
            }).when('/website/:Slug/users/create',
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteUsersForm.html',
                controller: 'websiteUsersFormController',
                controllerAs: 'webUsersForm'
            }).when('/website/:Slug/users/manage/:id',
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteUsersForm.html',
                controller: 'websiteUsersFormController',
                controllerAs: 'webUsersForm'
            }).when('/website/:Slug/coupons',               
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteCoupons.html',
                controller: 'websiteCouponsController',
                controllerAs: 'webCoupons'
            }).when('/website/:Slug/settings', //this will redirect to websiteSettings form - 
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websiteSettings/Index.html',
                controller: 'webSettingsController', //websiteSettings
                controllerAs: 'webSets'
            }).when('/website/:Slug/faq',
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteFaq.html',
                controller: 'websiteFaqController',
                controllerAs: 'webFaq'
            }).when("/website/:Slug/teams",
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteTeamsIndex.html',
                controller: 'websiteTeamsController',
                controllerAs: 'webTeams'
            }).when("/website/:Slug/teams/update/:id", 
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteTeamsForm.html',
                controller: 'websiteTeamsFormController',
                controllerAs: 'webTeamsForm'
            }).when("/website/:Slug/teams/create",
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteTeamsForm.html',
                controller: 'websiteTeamsFormController',
                controllerAs: 'webTeamsForm'
            }).when('/website/:Slug/contactrequests', //contactrequest, need to implement slug later?
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteContactRequest.html',
                controller: 'crAdminController',
                controllerAs: 'cradmin'
            }).when('/website/:Slug/contactrequests/spam', //contactrequest spam
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/websiteContactRequestSpam.html',
                controller: 'crSpamController',
                controllerAs: 'cradminspam'
            }).when("/website/:Slug/teams/schedule/:id",
            {
                templateUrl: 'Assets/Themes/bringpro/js/features/backoffice/templates/websites/teamJobSchedule.html',
                controller: 'scheduleController',
                controllerAs: 'sched'
            })
            $locationProvider.html5Mode(false).hashPrefix('');

        }])

})();