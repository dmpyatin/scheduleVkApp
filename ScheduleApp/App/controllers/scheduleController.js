(function () {
    var controllerId = 'scheduleController';
    app.controller(controllerId, [
        '$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

            var notifyHub = $.connection.notifyHub;

            $scope.connectionStarted = false;

            $.connection.hub.start().done(function () {
                $scope.connectionStarted = true;
            });

            notifyHub.client.callScheduleNotify = function (scheduleId) {
                for (var i = 0; i < $scope.unschedules.length; ++i) {
                    if ($scope.unschedules[i].id == scheduleId) {
                        window.location.reload();
                        $scope.$apply();
                        return;
                    }
                }

                for (var i = 0; i < $scope.schedules.length; ++i) {
                    if ($scope.schedules[i].id == scheduleId) {

                        //$scope.setMode($scope.mode);
                        window.location.reload();
                        //alert("Просматриваемые Вами данные были изменены другим пользователем, пожалуйста обновите страницу");
                        $scope.$apply();
                        return;
                    }
                }
            };

            $scope.changeScheduleNotify = function(scheduleId) {
                if ($scope.connectionStarted)
                    notifyHub.server.changeScheduleNotify(scheduleId);
            };

            var prefix = window.location.pathname;
            if (prefix[prefix.length - 1] != "/")
                prefix += "/";

            $scope.showTable = false;

            $scope.userName = window.pageModel.userName;
            $scope.isAuth = window.pageModel.isAuth;
            $scope.changeMode = false; 

            $scope.selectedSchedule = undefined;
            $scope.selectedDay = undefined;
            $scope.selectedPair = undefined;

            $scope.showFakeSchedule = false;

            $scope.resetViewSelected = function () {
                $scope.selectedSchedule = undefined;
                $scope.selectedDay = undefined;
                $scope.selectedPair = undefined;
                $scope.showFakeSchedule = false;
            }


            $scope.groupSelected = undefined;

            $scope.showNoFoundItem = false;

            $scope.schedules = [];
            $scope.unschedules = [];
            $scope.linkedSchedules = [];

            $scope.days = ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб"];
            $scope.daysFull = ["Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"];

            $scope.pairs = [1, 2, 3, 4, 5, 6, 7, 8];

            $scope.weekTypeNames = ["-", "Ч", "З"];

            $scope.mode = "students";
            $scope.view = "grid";

            if (window.pageModel.isAuth) {
                $cookies.changeMode = window.pageModel.userSettings.changeMode;
                $cookies.mode = window.pageModel.userSettings.mode;
                $cookies.view = window.pageModel.userSettings.view;
                $cookies.groupTitle = window.pageModel.userSettings.groupTitle;
                $cookies.lecturerTitle = window.pageModel.userSettings.lecturerTitle;
                $cookies.groupSelectedCode = window.pageModel.userSettings.groupSelectedCode;
                $cookies.groupSelectedSpecName = window.pageModel.userSettings.groupSelectedSpecName;
                $cookies.lecturerSelectedName = window.pageModel.userSettings.lecturerSelectedName;
                $scope.changeMode = $cookies.changeMode;
                if ($scope.changeMode)
                    $('.change').addClass('active');
            }
           

            $scope.$watch('groupSelected', function () {
                if ($scope.groupSelected !== undefined) {
                    if ($scope.groupSelected.code !== undefined && $scope.groupSelected.specialityName !== undefined) {
                        $scope.loadScheduleForGroup($scope.groupSelected.code, $scope.groupSelected.specialityName);
                        $scope.loadUnScheduleForGroup($scope.groupSelected.code, $scope.groupSelected.specialityName);

                        $cookies.groupSelectedCode = $scope.groupSelected.code;
                        $cookies.groupSelectedSpecName = $scope.groupSelected.specialityName;
                        $cookies.groupTitle = $scope.groupSelected.code + " " + $scope.groupSelected.specialityName;
                    }
                }
            });

            $scope.$watch('lecturerSelected', function () {
                if ($scope.lecturerSelected !== undefined) {
                    if ($scope.lecturerSelected.name !== undefined) {
                        $scope.loadScheduleForLecturer($scope.lecturerSelected.name);
                        $scope.loadUnScheduleForLecturer($scope.lecturerSelected.name);

                        $cookies.lecturerSelectedName = $scope.lecturerSelected.name;
                        $cookies.lecturerTitle = $scope.lecturerSelected.name;
                    }
                }
            });

        
            $scope.getGroups = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadGroups', {
                    params: {
                        template: val
                    }
                }).then(function (response) {              
                    return response.data;               
                });
            };

            $scope.getLecturers = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadLecturers', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                    return response.data;
                });
            };


            $scope.loadScheduleForGroup = function (groupCode, specialityName) {
                return $http.get(prefix + 'Schedule/GetScheduleForGroup', {
                    params: {
                        groupCode: groupCode,
                        specialityName: specialityName
                    }
                }).then(function (response) {

                    $scope.showTable = true;

                    $scope.resetViewSelected();

                    $scope.schedules = response.data;

                    if ($scope.schedules.length == 0) {
                        $scope.showNoFoundItem = true;
                    } else {
                        $scope.showNoFoundItem = false;
                    }

                    return response.data;
                });
            }

            $scope.loadScheduleForLecturer = function (lecturerName) {
                return $http.get(prefix + 'Schedule/GetScheduleForLecturer', {
                    params: {
                        lecturerName: lecturerName,
                        change: $scope.changeMode
                    }
                }).then(function (response) {

                    $scope.showTable = true;

                    $scope.resetViewSelected();

                    $scope.schedules = response.data;

                    if ($scope.schedules.length == 0) {
                        $scope.showNoFoundItem = true;
                    } else {
                        $scope.showNoFoundItem = false;
                    }

                    return response.data;
                });
            }

            $scope.loadUnScheduleForLecturer = function (lecturerName) {
                return $http.get(prefix + 'Schedule/GetUnscheduledSchedulesForLecturer', {
                    params: {
                        lecturerName: lecturerName
                    }
                }).then(function (response) {
                    $scope.unschedules = response.data;
                    return response.data;
                });
            }

            $scope.loadUnScheduleForGroup = function (groupCode, specialityName) {
                return $http.get(prefix + 'Schedule/GetUnscheduledSchedulesForGroup', {
                    params: {
                        groupCode: groupCode,
                        specialityName: specialityName
                    }
                }).then(function (response) {
                    $scope.unschedules = response.data;
                    return response.data;
                });
            }


            $scope.signIn = function (mail, code) {
                if (mail != undefined && mail != "" && code != undefined && code != "") {
                    return $http.get(prefix + 'Auth/SignIn', {
                        params: {
                            mail: mail,
                            code: code
                        }
                    }).then(function (response) {
                        if (response.data.id == undefined) {
                            $scope.signInMessage = response.data;
                        } else {
                            $scope.isAuth = true;
                            $scope.userName = response.data.mail;
                            $('#signInModal').modal('hide');
                        }

                        $cookies.changeMode = window.pageModel.userSettings.changeMode;
                        $cookies.mode = window.pageModel.userSettings.mode;
                        $cookies.view = window.pageModel.userSettings.view;
                        $cookies.groupTitle = window.pageModel.userSettings.groupTitle;
                        $cookies.lecturerTitle = window.pageModel.userSettings.lecturerTitle;
                        $cookies.groupSelectedCode = window.pageModel.userSettings.groupSelectedCode;
                        $cookies.groupSelectedSpecName = window.pageModel.userSettings.groupSelectedSpecName;
                        $cookies.lecturerSelectedName = window.pageModel.userSettings.lecturerSelectedName;

                        $scope.setMode($cookies.mode);
                        $scope.setView($cookies.view);

                        $scope.changeMode = $cookies.changeMode;
                        if ($scope.changeMode)
                             $('.change').addClass('active');

                        return response.data;
                    });
                }
            }


            $scope.register = function (mail, name) {
                if (mail != undefined && mail != "") {
                    return $http.get(prefix + 'Auth/Register', {
                        params: {
                            mail: mail,
                            name: name
                        }
                    }).then(function (response) {
                        $scope.registerMessage = response.data;
                        return response.data;
                    });
                }
            }

            $scope.signOut = function () {
                if ($scope.isAuth == true) {
                    return $http.get(prefix + 'Auth/SignOut', {
                    }).then(function (response) {
                        $scope.resetViewSelected();

                        if ($scope.changeMode)
                            $scope.setChangeMode();

                        $scope.isAuth = false;
                        $scope.userName = "";
                        return response.data;
                    });
                }
            }

            $scope.getSchedulesForCell = function (pair, day) {
                return $scope.schedules.filter(function (item) {
                    return (item.currentVersion.dayOfWeek == day && item.currentVersion.pairNumber == pair);
                });
            }

            $scope.getSchedulesForDay = function (day) {
                return $scope.schedules.filter(function (item) {
                    return (item.currentVersion.dayOfWeek == day);
                });
            }

            $scope.selectSchedule = function (schedule) {
                $scope.selectedSchedule = schedule;
            }

            $scope.selectCell = function (pair, day) {
                $scope.selectedDay = day;
                $scope.selectedPair = pair;
            }

            $scope.isSelectedCell = function (pair, day) {
                return $scope.selectedPair == pair && $scope.selectedDay == day;
            }

            $scope.selectDay = function (day) {
                $scope.selectedDay = day;
            }

            $scope.unscheduleSchedule = function(schedule){

                return $http.get(prefix + 'Schedule/UnscheduleSchedule', {
                    params: {
                        scheduleId: schedule.id
                    }
                }).then(function (response) {

                    $scope.changeScheduleNotify(schedule.id);
                    
                    if ($scope.mode == "students") {

                        if ($cookies.groupSelectedCode != undefined && $cookies.groupSelectedSpecName != undefined) {
                            $scope.loadUnScheduleForGroup($cookies.groupSelectedCode, $cookies.groupSelectedSpecName);
                        } else {
                            if ($scope.groupSelected != undefined) {
                                if ($scope.groupSelected.code != undefined && $scope.groupSelected.specialityName != undefined) {
                                    $scope.loadUnScheduleForGroup($scope.groupSelected.code, $scope.groupSelected.specialityName);
                                }
                            }
                        }
                    }

                    if ($scope.mode == "lecturers") {

                        if ($cookies.lecturerSelectedName != undefined) {
                            $scope.loadUnScheduleForLecturer($cookies.lecturerSelectedName);
                        } else {
                            if ($scope.lecturerSelected != undefined) {
                                if ($scope.lecturerSelected.name != undefined) {
                                    $scope.loadUnScheduleForLecturer($scope.lecturerSelected.name);
                                }
                            }
                        }
                    }

                    var index = $scope.schedules.indexOf(schedule);
                    if (index > -1) {
                        $scope.schedules.splice(index, 1);
                    }

                    return response.data;
                });
            }

            $scope.addSchedule = function (pair, day) {
 
            }

            $scope.pressAddButton = function () {
      
            }
            

            $scope.changeSchedule = function (schedule) {

                if ($scope.changeMode) {

                    $scope.changeModalTime = schedule.currentVersion.startTime + "-" + schedule.currentVersion.endTime;
                    $scope.changeModalAuditorium = schedule.currentVersion.auditoriumNumber + " " + schedule.currentVersion.buildingName;
                    //$scope.changeModalBuilding = schedule.currentVersion.buildingName;
                    $scope.changeModalTutorialName = schedule.currentVersion.tutorialName;
                    $scope.changeModalTutorialTypeName = schedule.currentVersion.tutorialTypeName;
                    $scope.changeModalLecturer = schedule.currentVersion.lecturerName;
                    $scope.changeModalSubGroup = schedule.currentVersion.subGroupName;
                    $scope.changeModalWeekType = schedule.currentVersion.weekTypeName;

                    $scope.changingModalSchedule = schedule;

                    $scope.errorModalMessages = undefined;


                    $('#changeScheduleModal').modal('show');
                }
            }

            $scope.pressChangeButton = function(){

                if ($scope.changeModalAuditorium == undefined || ( $scope.changeModalAuditorium.buildingShortName == undefined && $scope.changeModalAuditorium != undefined)) {
                    var spl = $scope.changeModalAuditorium.split(' ');
                    var auditoriumNumber = spl.splice(0, 1);
                    var buildingName = spl.join(' ');
                }

                if ($scope.changeModalTime.startTime == undefined) {
                    var spl = $scope.changeModalTime.split('-');
                    var startTime = spl[0] == undefined ? "" : spl[0];
                    var endTime = spl[1] == undefined ? "" : spl[1];
                }

                return $http.get(prefix + 'Schedule/ChangeSchedule', {
                    params: {
                        scheduleId: $scope.changingModalSchedule.id,
                        startTime: $scope.changeModalTime.startTime == undefined ? startTime : $scope.changeModalTime.startTime,
                        endTime: $scope.changeModalTime.endTime == undefined ? endTime : $scope.changeModalTime.endTime,
                        tutorialName: $scope.changeModalTutorialName.name == undefined ? $scope.changeModalTutorialName : $scope.changeModalTutorialName.name,
                        weekTypeName: $scope.changeModalWeekType.name == undefined ? $scope.changeModalWeekType : $scope.changeModalWeekType.name,
                        tutorialTypeName: $scope.changeModalTutorialTypeName.name == undefined ? $scope.changeModalTutorialTypeName : $scope.changeModalTutorialTypeName.name,
                        auditoriumNumber: ($scope.changeModalAuditorium == undefined || ($scope.changeModalAuditorium.buildingShortName == undefined && $scope.changeModalAuditorium != undefined)) ? auditoriumNumber : $scope.changeModalAuditorium.number,
                        buildingName: ($scope.changeModalAuditorium == undefined || ($scope.changeModalAuditorium.buildingShortName == undefined && $scope.changeModalAuditorium != undefined)) ? buildingName : $scope.changeModalAuditorium.buildingShortName,
                        subGroupName: $scope.changeModalSubGroup,
                        lecturerName: $scope.changeModalLecturer.name == undefined ? $scope.changeModalLecturer : $scope.changeModalLecturer.name
                    }
                }).then(function (response) {
          
                    $scope.changeScheduleNotify($scope.changingModalSchedule.id);

                    if (response.data.id == undefined) {
                        $scope.errorModalMessages = response.data;
                    } else {
                        for (var i = 0; i < $scope.schedules.length; ++i) {
                            if ($scope.schedules[i].id == response.data.id) {
                                $scope.schedules[i] = response.data;
                            }
                        }
                        $('#changeScheduleModal').modal('hide');
                    }

                    

                    return response.data;
                });

            }


       

            $scope.setShowFakeSchedule = function () {
                $scope.showFakeSchedule = !$scope.showFakeSchedule;
            }


            $scope.setChangeMode = function () {
                if ($scope.changeMode == false) {
                    if ($scope.isAuth) {


                        $scope.changeMode = true;
                        $('.change').addClass('active');

                        if($scope.mode == "lecturers")
                            window.location.reload();

                    } else {
                        $('#signInModal').modal('show');
                    }
                } else {
                    $scope.changeMode = false;
                    $('.change').removeClass('active');

                    if ($scope.mode == "lecturers")
                        window.location.reload();
                }

                $http.get(prefix + 'Schedule/SetChangeMode', {
                    params: {
                        changeMode: $scope.changeMode
                    }
                }).then(function (response) {
                });
            }


            

            $scope.setView = function (view) {
                $cookies.view = view;
                if (view !== undefined)
                    $scope.view = view;
                if ($scope.view == "grid") {
                    $('.list').removeClass('active');
                    $('.grid').addClass('active');
                }

                if ($scope.view == "list") {
                    $('.grid').removeClass('active');
                    $('.list').addClass('active');
                }

                $http.get(prefix + 'Schedule/SetView', {
                    params: {
                        view: $scope.view
                    }
                }).then(function (response) {
                });
            }

            $scope.setMode = function (mode) {         
                $cookies.mode = mode;

                $scope.schedules = [];

                if(mode !== undefined)
                    $scope.mode = mode;

                if ($scope.mode == "students") {



                    if ($cookies.groupSelectedCode !== undefined && $cookies.groupSelectedSpecName !== undefined) {
                        $scope.groupSelected = $cookies.groupTitle;
                        $scope.loadScheduleForGroup($cookies.groupSelectedCode, $cookies.groupSelectedSpecName);
                        $scope.loadUnScheduleForGroup($cookies.groupSelectedCode, $cookies.groupSelectedSpecName);
                    } else {
                        $scope.showTable = false;
                    }
                    $('.lecturers').removeClass('active');
                    $('.students').addClass('active');

                }

                if ($scope.mode == "lecturers") {
                    if ($cookies.lecturerSelectedName !== undefined) {
                        $scope.lecturerSelected = $cookies.lecturerTitle;
                        $scope.loadScheduleForLecturer($cookies.lecturerSelectedName);
                        $scope.loadUnScheduleForLecturer($cookies.lecturerSelectedName);
                    } else {
                        $scope.showTable = false;
                    }
                    $('.students').removeClass('active');
                    $('.lecturers').addClass('active');
                }

                $http.get(prefix + 'Schedule/SetMode', {
                    params: {
                        mode: $scope.mode
                    }
                }).then(function (response) {
                });
            }

            $scope.onInit = function () {
                $scope.setMode($cookies.mode);
                $scope.setView($cookies.view);
            }

            $scope.scheduleStartDrag = function (event, ui, schedule) {
                console.log("startDrag");
                $scope.draggedScheduleObj = schedule;

                $scope.getLinkedSchedules(schedule.id);
            }

            $scope.scheduleStopDrag = function (event, ui, schedule) {
                console.log("stopDrag");
                

                console.log($scope.schedules);
                console.log($scope.linkedSchedules);


                for (var i = 0; i < $scope.linkedSchedules.length; ++i) {
                    index = $scope.schedules.indexOf($scope.linkedSchedules[i]);
                    if (index > -1) {
                        $scope.schedules.splice(index, 1);
                    }
                }

                $scope.linkedSchedules = [];
                $scope.draggedScheduleObj = undefined;
            }

            $scope.unscheduleDrop = function (event, ui) {
                $scope.draggedScheduleUI = ui.draggable[0];
                $($scope.draggedScheduleUI).addClass("hide");
                $($scope.draggedScheduleUI)
                       .removeClass('hide')
                       .css("top", "")
                       .css("left", "0");
            }

            $scope.scheduleDrop = function (event, ui, scope, pair, day) {

                $scope.draggedScheduleUI = ui.draggable[0];

                if ($scope.draggedScheduleObj != undefined) {
                    var index = $scope.schedules.indexOf($scope.draggedScheduleObj);
                    if (index > -1) {
                        $scope.schedules.splice(index, 1);

                        if ($scope.draggedScheduleObj.currentVersion.pairNumber == pair &&
                            $scope.draggedScheduleObj.currentVersion.dayOfWeek == day) {

                            //magic
                            $($scope.draggedScheduleUI).addClass("hide");
                            $($scope.draggedScheduleUI)
                                   .removeClass('hide')
                                   .css("top", "")
                                   .css("left", "0");
                        }

                        $scope.draggedScheduleObj.currentVersion.pairNumber = pair;
                        $scope.draggedScheduleObj.currentVersion.dayOfWeek = day;
                        $scope.draggedScheduleObj.currentVersion.editor = $scope.userName;
                        $scope.schedules.push($scope.draggedScheduleObj);
                        $scope.setSchedulePairAndDay($scope.draggedScheduleObj, pair, day);
                    } else {
                        index = $scope.unschedules.indexOf($scope.draggedScheduleObj);
                        if (index > -1) {
                            $scope.unschedules.splice(index, 1);
                            $scope.draggedScheduleObj.currentVersion.pairNumber = pair;
                            $scope.draggedScheduleObj.currentVersion.dayOfWeek = day;
                            $scope.draggedScheduleObj.currentVersion.editor = $scope.userName;
                            $scope.schedules.push($scope.draggedScheduleObj);
                            $scope.setSchedulePairAndDay($scope.draggedScheduleObj, pair, day);
                        }
                    }
                }
            }

            $scope.setSchedulePairAndDay = function(schedule, pair, day){
                return $http.get(prefix + 'Schedule/SetSchedulePairAndDay', {
                    params: {
                        scheduleId: schedule.id,
                        pair: pair,
                        day: day
                    }
                }).then(function (response) {
                    $scope.changeScheduleNotify(schedule.id);
                });
            }


            $scope.getTimesModal = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadTimes', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                    return response.data;
                });
            }

            $scope.getAuditoriumsModal = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadAuditoriums', {
                    params: {
                        template: val,
                        pair : 0,
                        day : 0,
                        weekType: "",
                        tutorialType: ""
                    }
                }).then(function (response) {
                 
                    return response.data;
                });
            }

            $scope.getBuildingsModal = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadBuildings', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                    return response.data;
                });
            }

            $scope.getLecturersModal = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadLecturers', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                    return response.data;
                });
            }

            $scope.getTutorialsModal = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadTutorials', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                    return response.data;
                });
            }

            $scope.getTutorialTypesModal = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadTutorialTypes', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                    return response.data;
                });
            }
            $scope.getWeekTypesModal = function (val) {
                return $http.get(prefix + 'Schedule/TypeAheadWeekTypes', {
                    params: {
                        template: val
                    }
                }).then(function (response) {
                    return response.data;
                });
            }


            $scope.isLinkedSchedule = function (schedule) {
                for (var i = 0; i < $scope.linkedSchedules.length; ++i)
                    if ($scope.linkedSchedules[i].id == schedule.id)
                        return true;
                return false;
            }

            $scope.getLinkedSchedules = function (scheduleId) {

                
                $scope.linkedSchedules = [];

                return $http.get(prefix + 'Schedule/GetLinkedSchedules', {
                    params: {
                        scheduleId: scheduleId
                    }
                }).then(function (response) {

                                  
                    if ($scope.draggedScheduleObj != undefined) {
                        for (var i = 0; i < response.data.length; ++i) {
                            if ($scope.schedules.filter(function (x) { return x.id == response.data[i].id }).length == 0)
                                $scope.linkedSchedules.push(response.data[i]);
                        }

                        for (var i = 0; i < $scope.linkedSchedules.length; ++i)
                            $scope.schedules.push($scope.linkedSchedules[i]);

                    }
                    return response.data;
                });
            }

        }]);
})();