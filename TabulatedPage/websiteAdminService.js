(function () {
    "use strict"

    angular.module(APPNAME)
        .factory('$websiteAdminService', WebsiteAdminServiceFactory);

    WebsiteAdminServiceFactory.$inject = ['$baseService', '$bringPro'];

    function WebsiteAdminServiceFactory($baseService, $bringPro) {

        var aBringProServiceObject = bringPro.services.websites;


        var newService = $baseService.merge(true, {}, aBringProServiceObject, $baseService);

        return newService;
    }

})();