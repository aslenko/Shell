(function () {
    "use strict";

    /**
     * Security service
     */
    app.service('securityService', function ($http, $q, $cookies) {
        
        /**
         * Determines whether the current user is signed in.
         * @return a (resolved) promise
         */
        this.isSignedIn = function () {

            // both server and client consider the user
            // signed in if the user token cookie exists.
            // server will do the right thing in any case.

            var deferred = $q.defer();
            if (SLNK.isSignedIn()) {
                deferred.resolve();
            }
            else {
                deferred.reject({ unauthorized: true });
            }
            return deferred.promise;
        };      

    });

})();
