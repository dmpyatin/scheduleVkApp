(function () {
    var app = angular.module('schedule');
    var controllerId = 'reportController';
    app.controller(controllerId, [
        '$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

            var prefix = window.location.pathname;
            if (prefix[prefix.length - 1] != "/")
                prefix += "/";

            $scope.specialities = [];
            $scope.currentSpecialities;
            $scope.courses = [1, 2, 3, 4, 5, 6, 7];

            $scope.getSpecialities = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadSpecialities', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                    return response.data;
                });
            }

            $scope.addSpeciality = function(){
                $scope.specialities.push($scope.currentSpeciality);
            }
        }]);
})();