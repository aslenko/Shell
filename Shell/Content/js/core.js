(function () {
    "use strict";

    /**
     * Define a top level object to provide util functions etc.
     */
    var _core = {

        /**
         * Determines whether the specified object is not null (or undefined).
         * @param obj - the object to be verified
         * @return - true if defined, false otherwise
         */
        isNotNull: function (obj) {
            return SLNK.isNull(obj) === false;
        },

        /**
         * Determines whether the specified object is null (or undefined).
         * @param obj - the object to be verified
         * @return - true if null, false otherwise
         */
        isNull: function (obj) {
            return obj === null || typeof (obj) === 'undefined';
        },

        /**
         * Determines whether the specified string is null or empty.
         * @param str - the object to be verified
         * @return - true if null or empty string, false otherwise
         */
        isNullOrEmpty: function (str) {
            return SLNK.isNull(str) || str.length === 0;
        },

        /**
         * Get the specified cookie value.
         * @param name - the cookie name
         * @return - the cookie value
         */
        getCookie: function (name) {
            var val = null;
            var nameEq = name + "=";
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var c = cookies[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1, c.length);
                }
                if (c.indexOf(nameEq) === 0) {
                    val = c.substring(nameEq.length, c.length);
                    break;
                }
            }
            return val;
        },

        /**
         * Set the specified cookie value.
         * @param name - the cookie name
         * @param value - the cookie value
         * @param expires - the expiration date, expressed as utc string
         */
        setCookie: function (name, value, expires) {
            if (SLNK.isNull(expires)) {
                expires = '';
            }
            document.cookie = name + '=' + value + '; path=/' + '; expires = ' + expires;
        },

        /**
         * Clones the specified instance.
         * @param obj - the object to be cloned
         * @return - a clone of the specified object
         */
        clone: function (obj) {
            return JSON.parse(JSON.stringify(obj));
        },

        /**
         * Determines whether both objects are equal.
         * @param obj1 - the first object
         * @param obj2 - the second object
         * @return - true if object graphs are equal, false otherwise
         */
        equals: function (obj1, obj2) {
            return JSON.stringify(obj1) === JSON.stringify(obj2);
        },

        /**
         * Determine whether specified date range is active with respect to current UTC date.
         */
        isActive: function (validFrom, validTo) {
            var now = SLNK.nowUTC();
            var from = SLNK.isNull(validFrom) ? null : SLNK.parseUTC(validFrom);
            var to = SLNK.isNull(validTo) ? null : SLNK.parseUTC(validTo);
            return (from !== null && from < now) && (to === null || to > now);
        },

        /*
        * Returns the .NET representation of DateTime.MinValue (0001-01-01T00:00:00.000Z)
        */
        minDateString: function () {
            return "0001-01-01T00:00:00.000Z";
        },

        /*
        * Returns the current UTC Date string representation (2014-10-10T20:50:25.252Z)
        */
        nowUTCString: function () {
            return new Date().toJSON();
        },

        /*
        * Returns the current UTC Date
        */
        nowUTC: function () {
            return SLNK.parseUTC(SLNK.nowUTCString());
        },

        /*
        * Parses the string representation of the passed UTC Date and returns a date
        */
        parseUTC: function (d) {
            return new Date(d);
        },

        /**
         * Safely apply scope (checks whether digest is already running).
         * @param $scope - the angular scope
         */
        applyScope: function ($scope) {
            if (SLNK.isNull($scope.$$phase)) {
                $scope.$apply();
            }
        },

        /**
         * Create a fully qualified url using the service api base (defined in subway constants).
         * @param url - the relative url
         * @return - the fully qualified url
         */
        makeApiUrl: function (url) {
            url = $.trim(url);
            if (url.match("^/")) {
                url = url.substring(1);
            }
            return SLNK.Constants.ApiBase + url;
        },

        /*
        * Checks if there is a currently signed in user.
        * Both server and client consider the user signed in if the user token cookie exists.
        * Server will do the right thing in any case.
        */
        isSignedIn: function () {
            return SLNK.isNotNull(SLNK.getCookie(SLNK.Constants.UserToken));
        },

        /**
         * Get context information about the currently signed in user.
         * Consists of the following properties:
         * "ServiceLayerToken": Token used for communication with Service Layer
         * "JsonWebToken": Represents various claims for the currently logged in user
         * "PersonDetail": Person details for the currently logged in user
         * "RunAsPersonDetail": Person details for the "RunAs" user
         * @return - the user context
         */
        getUserContext: function () {
            var userContext = localStorage["SLNK.UserContext"];
            if (SLNK.isNotNull(userContext)) {
                return JSON.parse(userContext);
            }
            return null;
        },

        /**
         * Set context information about the currently signed in user.
         * Consists of the following properties:
         * "ServiceLayerToken": Token used for communication with Service Layer
         * "JsonWebToken": Represents various claims for the currently logged in user
         * "PersonDetail": Person details for the currently logged in user
         * "RunAsPersonDetail": Person details for the "RunAs" user
         * @param userContext - the user context
         */
        setUserContext: function (userContext) {
            localStorage["SLNK.UserContext"] = JSON.stringify(userContext);
        },

        /**
         * Get the service layer token for api calls.
         * @return - the token
         */
        getServiceLayerToken: function () {
            var userContext = SLNK.getUserContext();
            return SLNK.isNotNull(userContext) ? userContext.ServiceLayerToken : null;
        },

        /**
         * Set the service layer token for api calls.
         * @param serviceLayerToken - the (refreshed) service layer token.
         */
        setServiceLayerToken: function (serviceLayerToken) {
            var userContext = SLNK.getUserContext();
            if (SLNK.isNotNull(userContext)) {
                userContext.ServiceLayerToken = serviceLayerToken;
            }
            SLNK.setUserContext(userContext);
        },

        /**
        * Clears all client-related login information and redirect to Partners
        **/
        signOut: function () {
            //clear local storage
            localStorage.clear();

            //expire cookies
            var expr = new Date(0);
            for (var property in SLNK.Constants.SigninCookies) {
                if (SLNK.Constants.SigninCookies.hasOwnProperty(property)) {
                    SLNK.setCookie(SLNK.Constants.SigninCookies[property], '', expr.toUTCString());
                }
            }

            //redirect to Partners
            window.location.assign(SLNK.Constants.PartnersSignoutUrl);
        },

        /**
         * Refresh the service layer token for api calls.
         */
        refreshServiceLayerToken: function () {

            console.info(SLNK.nowUTCString() + " - Refreshing Service Layer Token...");

            $.ajax({
                type: "post",
                accepts: "json",
                dataType: "json",
                url: SLNK.Constants.ServiceLayerRefreshUrl,
                data: {},
                success: function (data, textStatus, jqXHR) {
                    console.info(SLNK.nowUTCString() + " - Refreshing Service Layer Token has been completed successfully!");
                    SLNK.setServiceLayerToken(data.ServiceLayerToken);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //sign out and redirect back to Partners
                    console.error(SLNK.nowUTCString() + " - Refreshing Service Layer Token has failed! (" + errorThrown + ")");
                    SLNK.signOut();
                }
            });
        },

        /**
         * Get the JSON web token for api calls.
         * @return - the token
         */
        getJsonWebToken: function () {
            var userContext = SLNK.getUserContext();
            return SLNK.isNotNull(userContext) ? userContext.JsonWebToken : null;
        },

        /**
         * Get the currently logged in person.
         * @return - the person detail
         */
        getPersonDetail: function () {
            var userContext = SLNK.getUserContext();
            return SLNK.isNotNull(userContext) ? userContext.PersonDetail : null;
        },

        /**
         * Get the RunAs person detail
         * @return - the person detail
         */
        getRunAsPersonDetail: function () {
            var userContext = SLNK.getUserContext();
            return SLNK.isNotNull(userContext) ? userContext.RunAsPersonDetail : null;
        },

        /**
        * Set the RunAs person detail to null        
        */
        clearRunAsPersonDetail: function () {
            var userContext = SLNK.getUserContext();
            if (SLNK.isNotNull(userContext)) {
                userContext.RunAsPersonDetail = null;
            }
            SLNK.setUserContext(userContext);
        },

        /**
        * Get either the RunAs (if available) or actual logged in user person detail
        * @return - the person detail
        */
        getCurrentContextPersonDetail: function () {
            var runAsPersonDetail = SLNK.getRunAsPersonDetail();

            if (SLNK.isNotNull(runAsPersonDetail)) {
                return runAsPersonDetail;
            }
            else {
                return SLNK.getPersonDetail();
            }
        },

        /*
        * Create request headers
        */
        createHeaders: function () {
            var acceptLanguage = SLNK.getCookie(SLNK.Constants.ApplicationLanguageCookie);
            var jsonWebToken = SLNK.getJsonWebToken();
            var serviceLayerToken = SLNK.getServiceLayerToken();

            return { "headers": { "Authorization": serviceLayerToken, "X-SLNK-JWT": jsonWebToken, "Accept-Language": acceptLanguage } };
        },

        /**
         * Performs an http get operation.
         * @param url - the target url
         * @param $http - the angular http service
         * @param $q - the angular q service
         * @param config - an [optional] http config
         * @return - a promise
         */
        get: function (url, $http, $q, config) {
            var deferred = $q.defer();
            var headers = SLNK.createHeaders();
            var extendedConfig = $.extend(true, config || {}, headers);
            $http.get(SLNK.makeApiUrl(url), extendedConfig)
               .success(function (retData, status, headers, reqConfig) { deferred.resolve({ 'data': retData, 'status': status, 'headers': headers, 'config': reqConfig }); })
               .error(function (retData, status, headers, reqConfig) { deferred.reject({ 'data': retData, 'status': status, 'headers': headers, 'config': reqConfig }); })
            return deferred.promise;
        },

        /**
         * Performs an http post operation.
         * @param url - the target url
         * @param data - the data to post
         * @param $http - the angular http service
         * @param $q - the angular q service
         * @param config - an [optional] http config
         * @return - a promise
         */
        post: function (url, data, $http, $q, config) {
            var deferred = $q.defer();
            var headers = SLNK.createHeaders();
            var extendedConfig = $.extend(true, config || {}, headers);
            $http.post(SLNK.makeApiUrl(url), data, extendedConfig)
               .success(function (retData, status, headers, reqConfig) { deferred.resolve({ 'data': retData, 'status': status, 'headers': headers, 'config': reqConfig }); })
               .error(function (retData, status, headers, reqConfig) { deferred.reject({ 'data': retData, 'status': status, 'headers': headers, 'config': reqConfig }); })
            return deferred.promise;
        },

        /**
         * Performs an http put operation.
         * @param url - the target url
         * @param data - the data to put
         * @param $http - the angular http service
         * @param $q - the angular q service
         * @param config - an [optional] http config
         * @return - a promise
         */
        put: function (url, data, $http, $q, config) {
            var deferred = $q.defer();
            var headers = SLNK.createHeaders();
            var extendedConfig = $.extend(true, config || {}, headers);
            $http.put(SLNK.makeApiUrl(url), data, extendedConfig)
               .success(function (retData, status, headers, reqConfig) { deferred.resolve({ 'data': retData, 'status': status, 'headers': headers, 'config': reqConfig }); })
               .error(function (retData, status, headers, reqConfig) { deferred.reject({ 'data': retData, 'status': status, 'headers': headers, 'config': reqConfig }); })
            return deferred.promise;
        },

        /**
         * Performs an http delete operation.
         * @param url - the target url
         * @param $http - the angular http service
         * @param $q - the angular q service
         * @param config - an [optional] http config
         * @return - a promise
         */
        delete: function (url, $http, $q, config) {
            var deferred = $q.defer();
            var headers = SLNK.createHeaders();
            var extendedConfig = $.extend(true, config || {}, headers);
            $http.delete(SLNK.makeApiUrl(url), extendedConfig)
               .success(function (retData, status, headers, reqConfig) { deferred.resolve({ 'data': retData, 'status': status, 'headers': headers, 'config': reqConfig }); })
               .error(function (retData, status, headers, reqConfig) { deferred.reject({ 'data': retData, 'status': status, 'headers': headers, 'config': reqConfig }); })
            return deferred.promise;
        },

        /**
         * Report an api error
         * @param response - the response, as given by $http.error (see above methods).
         */
        reportApiError: function (response) {
            // api messages come back in a (semi) predictable form.
            // as such look for message and status in known properties.
            // if a model state is included, report that accordingly.
            var showError = true;
            var message = "";
            //var message = 'API Error';
            //if (SLNK.isNotNull(response.status)) {
            //    if (response.status === 409)
            //    {
            //        //business rule violations
            //        showError = false;
            //        message = 'Conflict';
            //    }

            //    message += ' (' + response.status + ')';
            //}

            if (SLNK.isNotNull(response.data)) {
                if (typeof response.data == 'string') {
                    message += "<br>";
                    message += response.data;
                }
                else {
                    if (SLNK.isNotNull(response.data.Message)) {
                        message += "<br>";
                        message += response.data.Message;
                    }
                    if (SLNK.isNotNull(response.data.MessageDetail)) {
                        message += "<br>";
                        message += response.data.MessageDetail;
                    }
                    if (SLNK.isNotNull(response.data.ModelState)) {
                        $.each(response.data.ModelState, function (propertyName, propertyErrors) {
                            $.each(propertyErrors, function (index, propertyError) {
                                message += "<br>";
                                message += propertyError;
                            });
                        });
                    }
                }
            }

            if (showError)
                SLNK.showError(message);
            else
                SLNK.showWarning(message);
        },

        //this function can be used in place of reportApiError as a fix to ?'s being displayed in the Asian language (Korean, Chinese Traditional, and Chinese Simplified) error notifications instead of the appropriate characters
        getApiError: function (response) {
            var message = "";

            if (SLNK.isNotNull(response.data)) {
                if (typeof response.data == 'string') {
                    message += "<br>";
                    message += response.data;
                }
                else {
                    if (SLNK.isNotNull(response.data.Message)) {
                        message += response.data.Message;
                    }
                    if (SLNK.isNotNull(response.data.MessageDetail)) {
                        message += "<br>";
                        message += response.data.MessageDetail;
                    }
                    if (SLNK.isNotNull(response.data.ModelState)) {
                        $.each(response.data.ModelState, function (propertyName, propertyErrors) {
                            $.each(propertyErrors, function (index, propertyError) {
                                message += "<br>";
                                message += propertyError;
                            });
                        });
                    }
                }
            }

            return message;
        },

        /**
         * Show a notification.
         * @param message - the message
         */
        showSuccess: function (message) {
            SLNK.showNotification(message, "success", null);
        },

        /**
         * Show a notification.
         * @param message - the message
         */
        showInfo: function (message) {
            SLNK.showNotification(message, "info", null);
        },

        /**
         * Show a notification.
         * @param message - the message
         */
        showWarning: function (message) {
            SLNK.showNotification(message, "warning", null);
        },

        /**
         * Show a notification.
         * @param message - the message
         */
        showError: function (message) {
            SLNK.showNotification(message, "error", { allowHideAfter: 0, autoHideAfter: 0, hideOnClick: false });
        },

        /**
         * Show a notification.
         * @param message - the message
         * @param type - the [optional] message type (success, info, warning, error)
         * @param options - the [optional] configuration
         */
        showNotification: function (message, type, options) {
            var overrideOptions = options || {};
            var defaultOptions = {
                button: true,
                position: { left: 20, top: 65 },
                animation: {
                    open: {
                        effects: "slideIn:right"
                    },
                    close: {
                        effects: "slideIn:right",
                        reverse: true
                    }
                }
            }
            var notificationOptions = $.extend(true, {}, defaultOptions, overrideOptions);
            var notificationElement = $("#kendo-notification");
            notificationElement.kendoNotification(notificationOptions);
            var notificationWidget = notificationElement.data("kendoNotification");
            notificationWidget.show(message, type);
        },
    };

    window.SLNK = $.extend(window.SLNK, _core);

})();
