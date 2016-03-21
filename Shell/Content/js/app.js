(function () {
    "use strict";

    /*
     * Configure all routes in the application.
     * Its important that all routes are relative to app root.
     * The shell is the default server route, as such all template urls (below) are root relative. 
     */
    window.app = angular.module('SLNK_App', ['ngRoute', 'ngCookies', 'angular-loading-bar', 'validation.match', 'ngSanitize']);
    var isSignedIn = function (securityService) { return securityService.isSignedIn(); }

    app.config(function ($routeProvider) {
        // PLEASE NOTE:
        // The template url routes are defined in the _layout (razor) view
        // and set in the TemplateUrls object.  This is done so that the web
        // application base url is resolved properly via mvc razor (~).
        $routeProvider
            .when('/home', { templateUrl: SLNK.TemplateUrls.Home, controller: 'homeCtrl' })
            .when('/about', { templateUrl: SLNK.TemplateUrls.About, controller: 'aboutCtrl' })
            .when('/contact', { templateUrl: SLNK.TemplateUrls.Contact, controller: 'contactCtrl' })
            //.when('/role/defaults', { templateUrl: SLNK.TemplateUrls.RoleDefaults, controller: 'roleDefaultsCtrl', resolve: { 'isSignedIn': isSignedIn } })
            //.when('/person/detail', { templateUrl: SLNK.TemplateUrls.PersonDetail, controller: 'personCtrl', resolve: { 'isSignedIn': isSignedIn } })
            //.when('/person/detail/:id', { templateUrl: SLNK.TemplateUrls.PersonDetail, controller: 'personCtrl', resolve: { 'isSignedIn': isSignedIn } })
            //.when('/person/employees', { templateUrl: SLNK.TemplateUrls.Employees, controller: 'employeesCtrl', resolve: { 'isSignedIn': isSignedIn } })
            //.when('/person/overrides/:id', { templateUrl: SLNK.TemplateUrls.PersonOverrides, controller: 'overridesCtrl', resolve: { 'isSignedIn': isSignedIn } })
            //.when('/person/stores/:id', { templateUrl: SLNK.TemplateUrls.PersonStores, controller: 'storeAssignmentsCtrl', resolve: { 'isSignedIn': isSignedIn } })
            //.when('/user/password/:id', { templateUrl: SLNK.TemplateUrls.UserPassword, controller: 'userPasswordCtrl', resolve: { 'isSignedIn': isSignedIn } })
            //.when('/history/party-permission-override/:id', { templateUrl: SLNK.TemplateUrls.PartyPermissionOverrideHistory, controller: 'partyPermissionOverrideHistoryCtrl' })
            .otherwise({ redirectTo: '/person/employees' });
    });

    /*
     * Configure http provider for cross domain requests etc.
     */
    app.config(function ($httpProvider) {
        $httpProvider.defaults.useXDomain = true;
        $httpProvider.interceptors.push(function ($q) {
            return {
                responseError: function (response) {
                    //
                    // Angular removes the statusText (returned via xhr) when rejecting promises.
                    // As such, capture it here and move it to the data field (data is null on rejections anyway).
                    // See https://github.com/angular/angular.js/issues/2335
                    //
                    response.data = response.statusText;
                    return $q.reject(response);
                }
            }
        });
    });

    /*
     * Redirect to signin page when route change errors occur.
     * May occur if the server returns 401 (unauthorized).
     * May also occur if the user has signed out.
     */
    app.run(function ($rootScope, $location, $templateCache) {
        $rootScope.$on('$routeChangeError', function (event, current, previous, rejection) {
            if (rejection && rejection.status === 401) {
                location = SLNK.TemplateUrls.SignIn;
            }
            else if (rejection && rejection.unauthorized === true) {
                location = SLNK.TemplateUrls.SignIn;
            }
        });

        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            if (SLNK.isNotNull(next) && SLNK.isNotNull(next.$$route)) {
                console.log('$$route.templateUrl: ' + next.$$route.templateUrl);
            }
        });
    });

})();
