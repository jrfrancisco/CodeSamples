(function () {
    "use strict";

    angular.module(APPNAME)
        .controller("websiteAdminController", WebsiteAdminController);

    WebsiteAdminController.$inject = ['$scope', '$baseController', '$websiteAdminService', '$routeParams', '$location', '$rootScope', '$route']

    function WebsiteAdminController(
        $scope,
        $baseController,
        $websiteAdminService,
        $routeParams,
        $location,
        $rootScope,
        $route) {

        var vm = this;

        // - inheritance
        vm.$scope = $scope;
        vm.$baseController = $baseController;
        vm.$websiteAdminService = $websiteAdminService;
        vm.$routeParams = $routeParams;
        vm.$route = $route;
        vm.$location = $location;
        vm.$rootScope = $rootScope;
        vm.currentWebsite = {};
        vm.thisWebsite = {};

        vm.websiteSlug = $routeParams.Slug;

        vm.websiteAdminTabs = [
            {
                link: '#/website/:Slug/dashboard',
                label: 'dashboard',
                icon: "zmdi zmdi-account zmdi-hc-fww",
                urls: ["/website/:Slug/dashboard"]
            },
            {
                link: '#/website/:Slug/jobs',
                label: 'jobs',
                icon: "zmdi zmdi-truck zmdi-hc-fww",
                urls: ["/website/:Slug/jobs"]
            },
            {
                link: '#/website/:Slug/users',
                label: 'users',
                icon: "zmdi zmdi-truck zmdi-hc-fww",
                urls: ["/website/:Slug/users"]
            },
            {
                link: '#/website/:Slug/pricing',
                label: 'pricing',
                icon: "zmdi zmdi-money",
                urls: ["/website/:Slug/pricing"]
            },

            {
                link: '#/website/:Slug/coupons',
                label: 'coupons',
                icon: "zmdi zmdi-label-heart",
                urls: ["/website/:Slug/coupons"]
            },
            {
                link: '#/website/:Slug/settings',
                label: 'settings',
                icon: "zmdi zmdi-settings",
                urls: ["/website/:Slug/settings"]
            },
            {
                link: '#/website/:Slug/faq',
                label: 'faq',
                icon: "zmdi zmdi-info-outline",
                urls: ["/website/:Slug/faq"]
            },
            {
                link: '#/website/:Slug/contactrequests',
                label: 'contactrequests',
                icon: 'zmdi zmdi-info-outline',
                urls: ["/website/:Slug/contactrequests"]
            },
            {
                link: '#/website/:Slug/teams',
                label: 'teams',
                icon: "zmdi zmdi-account zmdi-hc-fww",
                urls: ["/website/:Slug/teams"]
            },




        ]

        vm.selectedTab = vm.websiteAdminTabs[0];
        vm.website = null;

        // - hoisting
        vm.setSelectedTab = _setSelectedTab;
        vm.tabClass = _tabClass;
        vm.onGetWebsiteSuccess = _onGetWebsiteSuccess;
        vm.onGetWebsiteError = _onGetWebsiteError;
        vm.onDropdownSubmit = _onDropdownSubmit;
        vm.onWebsiteGetByIdSuccess = _onWebsiteGetByIdSuccess;
        vm.onWebsiteGetByIdError - _onWebsiteGetByIdError;
        vm.onChange = _onChange;


        vm.$rootScope.$on("$routeChangeSuccess", function (event, data) {
            for (var i = 0; i < vm.websiteAdminTabs.length; i++) {

                var linkTab = vm.websiteAdminTabs[i].urls.indexOf(vm.$location.path());

                if (linkTab == -1) {
                    continue;
                } else {
                    _setSelectedTab(vm.websiteAdminTabs[i]);
                    break;
                }
            }
        });

        $baseController.merge(vm, $baseController);
        vm.notify = vm.$websiteAdminService.getNotifier($scope);

        renderWebsites();

        function renderWebsites() {
            vm.$websiteAdminService.get(vm.onGetWebsiteSuccess, vm.onGetWebsiteError)
        }

        function _onGetWebsiteSuccess(data) {
            vm.notify(function () {
                vm.website = data.items;
                vm.currentWebsite.slug = vm.website[0].slug;
                render();

            })

        }

        function _onGetWebsiteError(jqXhr, error) {
            console.error("error with website get", error);
        }

        function render() {
            vm.setUpCurrentRequest(vm);

            switch (vm.currentRequest.originalPath) {
                case "/website":
                    _onWebsiteTabClick();
                    break;

                case "/website/:Slug/dashboard":
                    vm.heading = "Website Admin Controller";
                    vm.message = "Website Admin.";
                    break;

                case "/website/:Slug/jobs":
                    vm.heading = "Jobs";
                    vm.message = "This text is coming from the main controller also but it's changing because of the new route. It's the same controller but it loads a different template into ng-view.";
                    break;

                case "/website/:Slug/users":
                    vm.heading = "Jobs";
                    vm.message = "This text is coming from the main controller also but it's changing because of the new route. It's the same controller but it loads a different template into ng-view.";
                    break;

                case "/website/:Slug/pricing":
                    vm.heading = "Pricing";
                    vm.message = "This text is coming from the main controller also but it's changing because of the new route. It's the same controller but it loads a different template into ng-view.";
                    break;

                case "/website/:Slug/coupons":
                    vm.heading = "Coupons";
                    vm.message = "This text is coming from the main controller also but it's changing because of the new route. It's the same controller but it loads a different template into ng-view.";
                    break;

                case "/website/:Slug/settings":
                    vm.heading = "Settings";
                    vm.message = "They are passed in the URL of the page on GET requests. Notice how Angular captures all of these params in a variable which you can access as $route.current.params.";
                    break;

                case "/website/:Slug/faq":
                    vm.heading = "FAQ";
                    vm.message = "This text is coming from the main controller also but it's changing because of the new route. It's the same controller but it loads a different template into ng-view.";
                    break;

                case "/website/contactrequests":
                    vm.heading = "Contact Request";
                    vm.message = "This text is coming from the main controller also but it's changing because of the new route. It's the same controller but it loads a different template into ng-view.";
                    break;

                case "/website/:Slug/teams":
                    vm.heading = "Teams";
                    vm.message = "This text is coming from the main controller also but it's changing because of the new route. It's the same controller but it loads a different template into ng-view.";
                    break;
            }
        }

        function _tabClass(tab) {
            if (vm.selectedTab == tab) {
                return "active";
            } else {
                return "";
            }
        }

        function _setSelectedTab(tab) {
            console.log("set selected tab", tab);
            vm.selectedTab = tab;

        }

        function _onDropdownSubmit() {
            vm.$websiteAdminService.getBySlug(vm.currentWebsite.slug, vm.onWebsiteGetByIdSuccess, vm.onWebsiteGetByIdError);

        }

        function _onWebsiteGetByIdSuccess(data) {
            console.log("check this website data", data);
            vm.notify(function () {
                vm.thisWebsite = data.item;
                _onChange();
            })

        }

        function _onWebsiteGetByIdError(jqXhr, error) {
            console.error("error with website get", error);

        }

        function _onChange() {
            var url = $location.path().split('/')[3];
            $location.path("/website/" + vm.thisWebsite.slug + "/" + url);
        }

        function _onWebsiteTabClick() {
            vm.selectedTab = vm.websiteAdminTabs[0];           
            $location.path("/website/" + vm.currentWebsite.slug + "/dashboard");

        }

    }
})();