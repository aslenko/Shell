(function () {
    "use strict";

    /**
     * Navbar controller.
     */
    app.controller('navBarCtrl', function ($scope, $cookieStore) {

        /**
         * Initialize the current instance.
         */
        $scope.initialize = function () {
        };

        /**
         * Called when signout button is clicked.
         */
        $scope.onSignOut = function () {
            //localStorage.clear();
            //$cookieStore.remove('CookieName');          
        };

    });

})();
