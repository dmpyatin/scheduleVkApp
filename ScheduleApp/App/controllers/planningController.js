(function () {
    var app = angular.module('schedule');
    var controllerId = 'planningController';
    app.controller(controllerId, [
        '$scope', '$http', '$cookies', function ($scope, $http, $cookies) {


            $scope.getGraph = function () {
                $http.post(prefix + 'Schedule/GetGraph', {})
                   .success(function (response) {
                       console.log("GRAPH");
                       console.log(response);

                       $('#container').remove();
                       $('#graph-container').html('<div id="container"></div>');
                      

                       s = new sigma({
                           graph: response,
                           container: 'container',
                           settings: {
                               defaultNodeColor: '#ec5148'
                           }
                       });
                   });
            }

            $scope.getGraphForSchedules = function () {

                $('#scheduleGraphModal').modal('show');

                if ($scope.groupCodesAndSpecNames != undefined) {
                    $http.post(prefix + 'Schedule/GetGraphForSchedules', {
                        groupCodesAndSpecNames: $scope.groupCodesAndSpecNames.join(','),
                        tutorialType: "",
                        planningType: ""
                    })
                       .success(function (response) {
                           console.log("GRAPH1");
                           console.log(response);

                           s = new sigma({
                               graph: response,
                               container: 'container',
                               settings: {
                                   defaultNodeColor: '#ec5148'
                               }
                           });
                       });
                }
            }


            var prefix = window.location.pathname;
            if (prefix[prefix.length - 1] != "/")
                prefix += "/";


            $scope.openThreadSelectorChart = function(){
                openThreadSelectorCounterChart($scope.schedulesCounter);
            }

            $scope.openQuartersSelectorChart = function () {
                openQuartersSelectorCounterChart($scope.quartersCounter);
            }
  
          

            $scope.schedules = [];
            $scope.currentSchedules = [];
            $scope.schedulesModalTitle = "";

            $scope.currentPlacings = [];

            

            $scope.daysFull = [{ name: "Понедельник", id: 0 },
                               { name: "Вторник", id: 1 },
                               { name: "Среда", id: 2 },
                               { name: "Четверг", id: 3 },
                               { name: "Пятница", id: 4 },
                               { name: "Суббота", id: 5 }];

       

            $scope.$watch('currentThreadSelector', function () {
                if ($scope.currentThreadSelector != undefined) {
                    $scope.getPlanningThreadProfile($scope.currentThreadSelector.name);
                }
            });

            $scope.$watch('currentDays', function () {
                $scope.daysIds = [];
                if ($scope.currentDays != undefined) {
                    for (var i = 0; i < $scope.currentDays.length; ++i) {
                        $scope.daysIds.push($scope.currentDays[i].id);
                    }

                    if ($scope.buildingAndAuditoriumNames.length > 0 && $scope.daysIds.length > 0 && $scope.timeNames.length > 0) {
                        $scope.getQuartersCounter($scope.buildingAndAuditoriumNames.join(';'), $scope.daysIds.join(';'), $scope.timeNames.join(';'));
                    }
                }
            });

            $scope.$watch('currentQuartersSelector', function () {
                if ($scope.currentQuartersSelector != undefined) {
                    console.log("CHANGE QUARTERS SELECTOR");
                    $scope.getPlanningQuartersProfile($scope.currentQuartersSelector.name);
                }
            });

            $scope.$watch('currentThreadSelectorName', function () {
                $scope.addThreadSelectorMessage = "";
                if ($scope.currentThreadSelectorName != undefined) {
                    if ($scope.currentThreadSelectorName.name != undefined) {
                        $scope.currentThreadSelectorName = $scope.currentThreadSelectorName.name;
                    }
                }
            });

            $scope.$watch('currentQuartersSelectorName', function () {
                $scope.addQuartersSelectorMessage = "";
                if ($scope.currentQuartersSelectorName != undefined) {
                    if ($scope.currentQuartersSelectorName.name != undefined) {
                        $scope.currentQuartersSelectorName = $scope.currentQuartersSelectorName.name;
                    }
                }
            });

            $scope.unselectThreadSelector = function () {
                $scope.currentThreadSelector = undefined;
            }

            $scope.unselectQuartersSelector = function () {
                $scope.currentQuartersSelector = undefined;
            }

            $scope.getPlanningThreadProfile = function (selectorName) {
                return $http.get(prefix + 'Schedule/GetPlanningThreadProfile', {
                    params: {
                        selectorName: selectorName
                    }
                }).then(function (response) {

                    $scope.deliveryUpdateGroups = response.data.groups;
                    $scope.currentGroups = [];
             
                    $scope.currentSpecialities = [];
                    for (var i = 0; i < response.data.specialities.length; i++) {
                        for (var j = 0; j < $scope.specialities.length; j++) {
                            if (response.data.specialities[i].id == $scope.specialities[j].id)
                                $scope.currentSpecialities.push($scope.specialities[j]);
                        }
                    }

                    return response.data;
                });
            }

            $scope.getPlanningQuartersProfile = function (selectorName) {
                return $http.get(prefix + 'Schedule/GetPlanningQuartersProfile', {
                    params: {
                        selectorName: selectorName
                    }
                }).then(function (response) {

                    console.log("SUCCESS");

                    console.log(response.data);

                    $scope.deliveryUpdateAuditoriums = response.data.auditoriums;
                    $scope.currentAuditoriums = [];

                    $scope.currentBuildings = [];
                    for (var i = 0; i < response.data.buildings.length; i++) {
                        for (var j = 0; j < $scope.buildings.length; j++) {
                            if (response.data.buildings[i].id == $scope.buildings[j].id)
                                $scope.currentBuildings.push($scope.buildings[j]);
                        }
                    }

                    $scope.currentDays = [];
                    for (var i = 0; i < response.data.days.length; i++) {
                        for (var j = 0; j < $scope.daysFull.length; j++) {
                            if (response.data.days[i] == $scope.daysFull[j].id)
                                $scope.currentDays.push($scope.daysFull[j]);
                        }
                    }

                    $scope.currentTimes = [];
                    for (var i = 0; i < response.data.times.length; i++) {
                        for (var j = 0; j < $scope.times.length; j++) {
                            if (response.data.times[i].id == $scope.times[j].id)
                                $scope.currentTimes.push($scope.times[j]);
                        }
                    }

                    return response.data;
                });
            }
       
            $scope.$watch('currentTimes', function () {
                $scope.timeNames = [];
                if ($scope.currentTimes !== undefined) {
                    for (var i = 0; i < $scope.currentTimes.length; ++i) {
                        $scope.timeNames.push($scope.currentTimes[i].startTime + "-" + $scope.currentTimes[i].endTime);
                    }

                    if ($scope.buildingAndAuditoriumNames.length > 0 && $scope.daysIds.length > 0 && $scope.timeNames.length > 0) {
                        $scope.getQuartersCounter($scope.buildingAndAuditoriumNames.join(';'), $scope.daysIds.join(';'), $scope.timeNames.join(';'));
                    }
                }
            });

            $scope.$watch('currentSpecialities', function () {
                $scope.specialityNames = [];

                //$scope.currentThreadSelector = undefined;

                $scope.schedulesCounter = undefined;

                if ($scope.currentSpecialities !== undefined) {
                    for (var i = 0; i < $scope.currentSpecialities.length; ++i) {
                        $scope.specialityNames.push($scope.currentSpecialities[i].name)
                    }
                    $scope.getAllGroups($scope.specialityNames.join(','));
                }
            });

            $scope.$watch('currentGroups', function () {
                console.log("changeCurrentGroups");

                //$scope.currentThreadSelector = undefined;

                $scope.groupCodes = [];

                $scope.groupCodesAndSpecNames = [];

                if ($scope.currentGroups !== undefined) {
                    for (var i = 0; i < $scope.currentGroups.length; ++i) {
                        $scope.groupCodes.push($scope.currentGroups[i].code)
                        $scope.groupCodesAndSpecNames.push($scope.currentGroups[i].code + '#' + $scope.currentGroups[i].specialityName);
                    }
                    if ($scope.groupCodesAndSpecNames.length > 0) {
                        $scope.getSchedulesCounter($scope.groupCodesAndSpecNames.join(','));
                    }
                }        
            });

            $scope.$watch('currentBuildings', function () {
                $scope.buildingNames = [];

                if ($scope.currentBuildings !== undefined) {
                    for (var i = 0; i < $scope.currentBuildings.length; ++i) {
                        $scope.buildingNames.push($scope.currentBuildings[i].shortName);      
                    }
                    $scope.getAllAuditoriums($scope.buildingNames.join(';'));
                }
            });

            $scope.$watch('currentAuditoriums', function () {
                $scope.auditoriumNumbers = [];

                $scope.buildingAndAuditoriumNames = [];

                if ($scope.currentAuditoriums !== undefined) {
                    for (var i = 0; i < $scope.currentAuditoriums.length; ++i) {
                        $scope.auditoriumNumbers.push($scope.currentAuditoriums[i].number);
                        $scope.buildingAndAuditoriumNames.push($scope.currentAuditoriums[i].number + '#' + $scope.currentAuditoriums[i].buildingShortName);
                    }

                    if ($scope.buildingAndAuditoriumNames.length > 0 && $scope.daysIds.length > 0 && $scope.timeNames.length > 0) {
                        $scope.getQuartersCounter($scope.buildingAndAuditoriumNames.join(';'), $scope.daysIds.join(';'), $scope.timeNames.join(';'));
                    }
                }
            });



            $scope.getAllSpecialities = function () {
                return $http.get(prefix + 'Schedule/GetAllSpecialities', {
                }).then(function (response) {
                    $scope.specialities = response.data;
                    return response.data;
                });
            }

            $scope.getAllThreadSelectors = function (presetSelectorName) {
                return $http.get(prefix + 'Schedule/GetAllThreadProfileSelectors', {
                }).then(function (response) {
                    $scope.threadSelectors = response.data;

                    if(presetSelectorName != undefined){
                        for (var i = 0; i < $scope.threadSelectors.length; ++i) {
                            if ($scope.threadSelectors[i].name == presetSelectorName) {
                                $scope.currentThreadSelector = $scope.threadSelectors[i];
                                break;
                            }
                        }
                    }

                    return response.data;
                });
            }

            $scope.getAllQuartersSelectors = function (presetSelectorName) {
                return $http.get(prefix + 'Schedule/GetAllQuartersProfileSelectors', {
                }).then(function (response) {
                    $scope.quartersSelectors = response.data;

                    if (presetSelectorName != undefined) {
                        for (var i = 0; i < $scope.quartersSelectors.length; ++i) {
                            if ($scope.quartersSelectors[i].name == presetSelectorName) {
                                $scope.currentQuartersSelector = $scope.quartersSelectors[i];
                                break;
                            }
                        }
                    }

                    return response.data;
                });
            }

            $scope.getAllTimes = function () {
                return $http.get(prefix + 'Schedule/GetAllTimes', {
                }).then(function (response) {
                    $scope.times = response.data;
                    return response.data;
                });
            }

            $scope.getAllBuildings = function () {
                return $http.get(prefix + 'Schedule/GetAllBuildings', {
                }).then(function (response) {
                    $scope.buildings = response.data;
                    return response.data;
                });
            }

            $scope.getAllAuditoriums = function (buildingNames) {

                $http.post(prefix + 'Schedule/GetAllAuditoriums', {
                    buildingNames: buildingNames
                })
                   .success(function (response) {

                    $scope.auditoriums = response;

                    if ($scope.deliveryUpdateAuditoriums.length > 0) {
                        for (var i = 0; i < $scope.deliveryUpdateAuditoriums.length; i++) {
                            for (var j = 0; j < $scope.auditoriums.length; j++) {
                                if ($scope.deliveryUpdateAuditoriums[i].id == $scope.auditoriums[j].id)
                                    $scope.currentAuditoriums.push($scope.auditoriums[j]);
                            }
                        }

                        $scope.buildingAndAuditoriumNames = [];

                        for (var i = 0; i < $scope.currentAuditoriums.length; ++i) {
                            $scope.buildingAndAuditoriumNames.push($scope.currentAuditoriums[i].number + '#' + $scope.currentAuditoriums[i].buildingShortName);
                        }

                        if ($scope.buildingAndAuditoriumNames.length > 0 && $scope.daysIds.length > 0 && $scope.timeNames.length > 0) {
                            $scope.getQuartersCounter($scope.buildingAndAuditoriumNames.join(';'), $scope.daysIds.join(';'), $scope.timeNames.join(';'));
                        }

                        $scope.deliveryUpdateAuditoriums = [];
                    }


                    return response.data;
                });
            }


            $scope.getAllGroups = function (specialityNames) {

                $http.post(prefix + 'Schedule/GetAllGroups', {
                        specialities: specialityNames,
                    })
                    .success(function (response) {
                    
                        $scope.groups = response;

                        if ($scope.deliveryUpdateGroups.length > 0) {
                            for (var i = 0; i < $scope.deliveryUpdateGroups.length; i++) {
                                for (var j = 0; j < $scope.groups.length; j++) {
                                    if ($scope.deliveryUpdateGroups[i].id == $scope.groups[j].id)
                                        $scope.currentGroups.push($scope.groups[j]);
                                }
                            }

                            $scope.groupCodes = [];
                            $scope.groupCodesAndSpecNames = [];

                            for (var i = 0; i < $scope.currentGroups.length; ++i) {
                                $scope.groupCodes.push($scope.currentGroups[i].code)
                                $scope.groupCodesAndSpecNames.push($scope.currentGroups[i].code + '#' + $scope.currentGroups[i].specialityName);
                            }
                            if ($scope.groupCodes.length > 0) {
                                $scope.getSchedulesCounter($scope.groupCodesAndSpecNames.join(','));
                            }

                            $scope.deliveryUpdateGroups = [];
                        }
                });
            }

            $scope.getSchedulesCounter = function (groupCodesAndSpecNames) {
                $http.post(prefix + 'Schedule/GetSchedulesCounterForGroups', {
                    groupCodesAndSpecNames: groupCodesAndSpecNames,
                }).success(function (response) {
                    $scope.schedulesCounter = response;
                });
            }

            $scope.getQuartersCounter = function (buildingAndAuditoriumNames, days, times) {
                $http.post(prefix + 'Schedule/GetQuartersCounter', {
                    buildingAndAuditoriumNames: buildingAndAuditoriumNames,
                    days: days,
                    times: times
                }).success(function (response) {
                    $scope.quartersCounter = response;
                });
            }

            $scope.addThreadSelectorModal = function () {
                $http.post(prefix + 'Schedule/AddPlanningThreadProfileSelector', {
                    selectorName: $scope.currentThreadSelectorName,
                    specialityNames: $scope.specialityNames.join(';'),
                    groupCodes: $scope.groupCodes.join(';')
                })
                   .success(function (response) {
                       if (response.groups != undefined) {
                           $scope.getAllThreadSelectors($scope.currentThreadSelectorName);
                           $('#addThreadSelectorModal').modal('hide');
                       } else {
                           $scope.addThreadSelectorMessage = response;
                       }
                   });
            }

            $scope.addQuartersSelectorModal = function () {
                $http.post(prefix + 'Schedule/AddPlanningQuartersProfileSelector', {
                    selectorName: $scope.currentQuartersSelectorName,
                    buildingNames: $scope.buildingNames.join(';'),
                    auditoriumNumbers: $scope.auditoriumNumbers.join(';'),
                    days: $scope.daysIds.join(';'),
                    times: $scope.timeNames.join(';')
                })
                   .success(function (response) {
                       if (response.buildings != undefined) {
                           $scope.getAllQuartersSelectors($scope.currentQuartersSelectorName);
                           $('#addQuartersSelectorModal').modal('hide');
                       } else {
                           $scope.addQuartersSelectorMessage = response;
                       }
                   });
            }


            $scope.getPlacingsForSelector = function (auditoriumType, viewType) {
                $http.post(prefix + 'Schedule/GetPlacingsForSelector', {
                    buildingAndAuditoriumNames: $scope.buildingAndAuditoriumNames.join(';'),
                    days: $scope.daysIds.join(';'),
                    times: $scope.timeNames.join(';'),
                    auditoriumType: auditoriumType,
                    viewType: viewType
                }).success(function (response) {
                    console.log("PLACINGS");
                    console.log(response);
                    $scope.placings = response;

                    $scope.currentPlacings = [];
                    $('#placingsForSelectorModal').modal('show');
                });
            }

            $scope.getSchedulesForSelector = function (tutorialType, planningType) {
                $http.post(prefix + 'Schedule/GetSchedulesForSelector', {
                    groupCodesAndSpecNames: $scope.groupCodesAndSpecNames.join(','),
                    tutorialType: tutorialType,
                    planningType: planningType
                }).success(function (response) {

                    $scope.currentModalPlanningType = planningType;

                    if(tutorialType == "Лек"){
                        $scope.schedulesModalTitle = "Лекции";
                    }
                    if (tutorialType == "Прак") {
                        $scope.schedulesModalTitle = "Практики";
                    }
                    if (tutorialType == "Лаб") {
                        $scope.schedulesModalTitle = "Лабораторные";
                    }
                    if (tutorialType == "other") {
                        $scope.schedulesModalTitle = "Другое";
                    }
                    if (tutorialType == "all") {
                        $scope.schedulesModalTitle = "Общее";
                    }

                    $scope.schedules = response.sort(function (a, b) {
                        if (a.currentVersion.tutorialName < b.currentVersion.tutorialName)
                            return -1;
                        if (a.currentVersion.tutorialName > b.currentVersion.tutorialName)
                            return 1;
                        return 0;
                    });

                    console.log(response);

                    $scope.currentSchedules = [];
                    $scope.currentPlacings = [];

                    $('#schedulesForSelectorModal').modal('show');
                    
                });
            }

            $scope.unplanSchedules = function () {
                if ($scope.currentSchedules.length > 0) {
                    var scheduleIds = [];

                    for (var i = 0; i < $scope.currentSchedules.length; ++i) {
                        scheduleIds.push($scope.currentSchedules[i].id);
                    }

                    $http.post(prefix + 'Schedule/UnplanningSchedulesForSelector', {
                        scheduleIds: scheduleIds.join(',')
                    }).success(function (response) {

                        if ($scope.groupCodesAndSpecNames != undefined && $scope.groupCodesAndSpecNames.length > 0) {
                            $scope.getSchedulesCounter($scope.groupCodesAndSpecNames.join(','));
                        }
                    });
                }
            }

            $scope.planSchedules = function () {

                if ($scope.currentSchedules.length > 0 && $scope.buildingAndAuditoriumNames.length > 0 && $scope.daysIds.length > 0 && $scope.timeNames.length > 0) {

                    var scheduleIds = [];

                    for (var i = 0; i < $scope.currentSchedules.length; ++i) {
                        scheduleIds.push($scope.currentSchedules[i].id);
                    }


                    $http.post(prefix + 'Schedule/PlanningSchedulesForSelector', {
                        scheduleIds: scheduleIds.join(','),
                        buildingAndAuditoriumNames: $scope.buildingAndAuditoriumNames.join(';'),
                        days: $scope.daysIds.join(';'),
                        times: $scope.timeNames.join(';')
                    }).success(function (response) {
                        console.log("PLANNING IS OK");

                        if ($scope.groupCodesAndSpecNames != undefined && $scope.groupCodesAndSpecNames.length > 0) {
                            $scope.getSchedulesCounter($scope.groupCodesAndSpecNames.join(','));
                        }

                    });
                }
            }


            $scope.getThreadSelectorsModal = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadThreadSelectors', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                   
                    return response.data;
                });
            }

            $scope.getQuartersSelectorsModal = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadQuartersSelectors', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                    return response.data;
                });
            }

            $scope.addThreadSelector = function () {
                $scope.addThreadSelectorMessage = "";
            }

            $scope.dellThreadSelector = function () {
                return $http.get(prefix + 'Schedule/DellPlanningThreadProfileSelector', {
                    params: {
                        selectorName: $scope.currentThreadSelector.name
                    }
                }).then(function (response) {
                    $scope.onInitThreads();
                    return response.data;
                });
            }

            $scope.dellQuartersSelector = function () {
                return $http.get(prefix + 'Schedule/DellPlanningQuartersProfileSelector', {
                    params: {
                        selectorName: $scope.currentQuartersSelector.name
                    }
                }).then(function (response) {
                    $scope.onInitQuarters();
                    return response.data;
                });
            }



            $scope.addQuartersSelector = function () {
                $scope.addQuartersSelectorMessage = "";
            }

            $scope.onInitThreads = function () {

                $scope.specialities = [];
                $scope.groups = [];
                $scope.specialityNames = [];
                $scope.groupCodes = [];
                $scope.groupCodesAndSpecNames = [];
                $scope.schedulesCounter = {};
                $scope.currentGroups = [];
                $scope.currentSpecialities = [];
                $scope.deliveryUpdateGroups = [];

                $scope.currentThreadSelector = undefined;
                $scope.threadSelectors = [];

                $scope.getAllSpecialities();
                $scope.getAllThreadSelectors();
            }


            $scope.onInitQuarters = function () {
                $scope.buildings = [];
                $scope.auditoriums = [];

                $scope.buildingNames = [];
                $scope.auditoriumNumbers = [];
                $scope.quartersCounter = {};
                $scope.buildingAndAuditoriumNames = [];
                $scope.deliveryUpdateAuditoriums = [];

                $scope.times = [];
                $scope.timeNames = [];

                $scope.currentDays = [];
                $scope.currentTimes = [];

                $scope.currentQuartersSelector = undefined;
                $scope.quartersSelectors = [];

                $scope.daysIds = [];

                $scope.placings = [];

                $scope.getAllBuildings();
                $scope.getAllTimes();
                $scope.getAllQuartersSelectors();
            }

            $scope.onInit = function () {      
                $scope.onInitThreads();
                $scope.onInitQuarters();

                //$scope.getGraph();
            }

        }]);
})();


