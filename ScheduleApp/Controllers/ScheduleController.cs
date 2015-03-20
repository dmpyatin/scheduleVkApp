using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScheduleData.Services;
using ScheduleApp.Infrastructure;
using ScheduleApp.Models;
using ScheduleData.Models;
using ScheduleData.Exceptions;
using ScheduleApp.Hubs;

namespace ScheduleApp.Controllers
{
    public class ScheduleController : Controller
    {
        DataService _dataService;
        UserService _userService;


        public ActionResult Schedule()
        {

           

            var userSettings = new UserSettings();
            if (User.Identity.IsAuthenticated)
            {
                _userService = new UserService();
                userSettings = _userService.GetUserSettings(User.Identity.Name);
            }


            var model = new ScheduleViewModel()
            {
                UserName = User.Identity.Name,
                IsAuth = User.Identity.IsAuthenticated,
                UserSettings = userSettings
            };

            return View(model);
        }

        public ActionResult GetLinkedSchedules(string scheduleId)
        {
            _dataService = new DataService();
            var result = _dataService.GetLinkedSchedules(scheduleId);
            return new JsonNetResult(result);
        }

        public ActionResult TypeAheadGroups(string template)
        {
            _dataService = new DataService();
            var result = _dataService.GetGroupsByFirstMatching(template, 15);
            return new JsonNetResult(result);
        }

        public ActionResult TypeAheadTimes(string template)
        {
            _dataService = new DataService();
            var result = _dataService.GetTimesByFirstMatching(template);
            return new JsonNetResult(result);
        }

        public ActionResult TypeAheadAuditoriums(
            string template,
            int pair,
            int day,
            string weekType,
            string tutorialType
            )
        {
            _dataService = new DataService();
            var result = _dataService.GetAuditoriumsByFirstMatching(template, pair, day, weekType, tutorialType);
            return new JsonNetResult(result);
        }

        public ActionResult TypeAheadWeekTypes(string template)
        {
            _dataService = new DataService();
            var result = _dataService.GetWeekTypesByFirstMatching(template);
            return new JsonNetResult(result);
        }

        public ActionResult TypeAheadBuildings(string template)
        {
            _dataService = new DataService();
            var result = _dataService.GetBuildingsByFirstMatching(template);
            return new JsonNetResult(result);
        }

        public ActionResult TypeAheadThreadSelectors(string template)
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();
                var result = _dataService.GetThreadSelectorsByFirstMatching(template, User.Identity.Name);
                return new JsonNetResult(result);
            }
            return new JsonNetResult();
        }

        public ActionResult TypeAheadQuartersSelectors(string template)
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();
                var result = _dataService.GetQuartersSelectorsByFirstMatching(template, User.Identity.Name);
                return new JsonNetResult(result);
            }
            return new JsonNetResult();
        }

        public ActionResult TypeAheadTutorials(string template)
        {
            _dataService = new DataService();
            var result = _dataService.GetTutorialsByFirstMatching(template);
            return new JsonNetResult(result);
        }

        public ActionResult TypeAheadTutorialTypes(string template)
        {
            _dataService = new DataService();
            var result = _dataService.GetTutorialTypesByFirstMatching(template);
            return new JsonNetResult(result);
        }

        public ActionResult TypeAheadSpecialities(string template)
        {
            _dataService = new DataService();
            var result = _dataService.GetSpecialitiesByFirstMatching(template);
            return new JsonNetResult(result);
        }

        public ActionResult TypeAheadLecturers(string template)
        {
            _dataService = new DataService();
            var result = _dataService.GetLecturersByFirstMatching(template, 15);
            return new JsonNetResult(result);
        }

        public ActionResult GetScheduleForGroup(string groupCode, string specialityName)
        {

            if (User.Identity.IsAuthenticated)
            {
                _userService = new UserService();
                var userSettings = _userService.GetUserSettings(User.Identity.Name);

                userSettings.GroupSelectedCode = groupCode;
                userSettings.GroupSelectedSpecName = specialityName;
                userSettings.GroupTitle = groupCode + " " + specialityName;

                _userService.SaveUserSettings(User.Identity.Name, userSettings);
            }


            _dataService = new DataService();
            var result = _dataService.GetSchedulesForGroup(groupCode, specialityName);
            return new JsonNetResult(result);
        }

        public ActionResult GetScheduleForLecturer(string lecturerName, bool change)
        {
            if (User.Identity.IsAuthenticated)
            {
                _userService = new UserService();
                var userSettings = _userService.GetUserSettings(User.Identity.Name);

                userSettings.LecturerSelectedName = lecturerName;
                userSettings.LecturerTitle = lecturerName;

                _userService.SaveUserSettings(User.Identity.Name, userSettings);
            }

            _dataService = new DataService();
            var result =  change ? _dataService.GetSchedulesForLecturer(lecturerName) : _dataService.GetAgregateSchedulesForLecturer(lecturerName);
            return new JsonNetResult(result);
        }



        public ActionResult GetUnscheduledSchedulesForLecturer(string lecturerName)
        {
            _dataService = new DataService();
            var result = _dataService.GetUnscheduledSchedulesForLecturer(lecturerName);
            return new JsonNetResult(result);
        }

        public ActionResult GetUnscheduledSchedulesForGroup(string groupCode, string specialityName)
        {
            _dataService = new DataService();
            var result = _dataService.GetUnscheduledSchedulesForGroup(groupCode, specialityName);
            return new JsonNetResult(result);
        }

        public void UnscheduleSchedule(string scheduleId)
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();
                _dataService.UnscheduleSchedule(scheduleId, User.Identity.Name);
            }
            else
            {
                //...
            }
        }


        public ActionResult ChangeSchedule(string scheduleId,
                                   string startTime,
                                   string endTime,
                                   string tutorialName,
                                   string weekTypeName,
                                   string tutorialTypeName,
                                   string auditoriumNumber,
                                   string buildingName,
                                   string subGroupName,
                                   string lecturerName)
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();

                try
                {
                   var schedule = _dataService.ChangeSchedule(
                    scheduleId,
                    startTime,
                    endTime,
                    tutorialName,
                    weekTypeName,
                    tutorialTypeName,
                    auditoriumNumber,
                    buildingName,
                    subGroupName,
                    lecturerName,
                    User.Identity.Name);

                    return new JsonNetResult(schedule);

                }
                catch (EntityNotFoundException ex)
                {
                    //...

                    return new JsonNetResult(ex.Messages);
                }
              
            }
            else
            {
                //...


                return new JsonNetResult(null);
            }
        }


        //TODO: Improve
        /*
        public void AddSchedule(string startTime,
                                string endTime,
                                string tutorialName,
                                string weekTypeName,
                                string tutorialTypeName,
                                string lecturerName,
                                string groupCode,
                                string auditoriumNumber,
                                string buildingName,
                                string subGroupName,
                                int pairNumber,
                                int dayOfWeek
            ){
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();

                try
                {
                    _dataService.AddSchedule(startTime.Trim(),
                                             endTime.Trim(),
                                             tutorialName.Trim(),
                                             weekTypeName.Trim(),
                                             tutorialTypeName.Trim(),
                                             lecturerName.Trim(),
                                             groupCode.Trim(),
                                             auditoriumNumber.Trim(),
                                             buildingName.Trim(),
                                             subGroupName.Trim(),
                                             User.Identity.Name,
                                             pairNumber,
                                             dayOfWeek);
                }catch(EntryPointNotFoundException ex){
                    //...
                }
            }
            else
            {
                //...
            }
        }*/

      

        public void SetSchedulePairAndDay(string scheduleId, int pair, int day)
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();
                _dataService.SetSchedulePairAndDay(scheduleId, pair, day, User.Identity.Name);
            }
            else
            {

            }
        }

        public void SetMode(string mode)
        {
            if (User.Identity.IsAuthenticated)
            {
                _userService = new UserService();
                var userSettings = _userService.GetUserSettings(User.Identity.Name);

                userSettings.Mode = mode;
 
                _userService.SaveUserSettings(User.Identity.Name, userSettings);
            }
        }

        public void SetView(string view)
        {
            if (User.Identity.IsAuthenticated)
            {
                _userService = new UserService();
                var userSettings = _userService.GetUserSettings(User.Identity.Name);

                userSettings.View = view;

                _userService.SaveUserSettings(User.Identity.Name, userSettings);
            }
        }

        public void SetChangeMode(bool changeMode)
        {
            if (User.Identity.IsAuthenticated)
            {
                _userService = new UserService();
                var userSettings = _userService.GetUserSettings(User.Identity.Name);

                userSettings.ChangeMode = changeMode;

                _userService.SaveUserSettings(User.Identity.Name, userSettings);
            }
        }


        public ActionResult GetAllSpecialities()
        {
            _dataService = new DataService();
            var result = _dataService.GetAllSpecialities();
            return new JsonNetResult(result);
        }

        public ActionResult GetAllGroups(string specialities)
        {
            _dataService = new DataService();

            var splits = specialities.Split(',');

            var result = _dataService.GetAllGroups(splits.ToList());
            return new JsonNetResult(result);
        }

        public ActionResult GetAllBuildings(){
            _dataService = new DataService();
            var result = _dataService.GetAllBuildings();
            return new JsonNetResult(result);
        }

        public ActionResult GetAllAuditoriums(string buildingNames)
        {
            _dataService = new DataService();
            var splits = buildingNames.Split(';');
            var result = _dataService.GetAllAuditoriums(splits.ToList());
            return new JsonNetResult(result);
        }

        public ActionResult GetAllThreadProfileSelectors()
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();
                return new JsonNetResult(_dataService.GetAllThreadProfileSelectors(User.Identity.Name));
            }

            //TODO
            return new JsonNetResult();
        }

        public ActionResult GetAllQuartersProfileSelectors()
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();
                return new JsonNetResult(_dataService.GetAllQuartersProfileSelectors(User.Identity.Name));
            }

            //TODO
            return new JsonNetResult();
        }

        public ActionResult GetPlanningThreadProfile(string selectorName)
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();
                return new JsonNetResult(_dataService.GetPlanningThreadProfile(selectorName, User.Identity.Name));
            }

            //TODO
            return new JsonNetResult();
        }

        public ActionResult GetPlanningQuartersProfile(string selectorName)
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();
                return new JsonNetResult(_dataService.GetPlanningQuartersProfile(selectorName, User.Identity.Name));
            }

            //TODO
            return new JsonNetResult();
        }


        public ActionResult GetSchedulesCounterForGroups(string groupCodesAndSpecNames)
        {
            _dataService = new DataService();

            var splits = groupCodesAndSpecNames.Split(',');

            var result = _dataService.GetSchedulesCounterForGroups(splits.ToList());
            return new JsonNetResult(result);
        }

        public ActionResult GetQuartersCounter(string buildingAndAuditoriumNames, string days, string times)
        {
            _dataService = new DataService();

            var splits1 = buildingAndAuditoriumNames.Split(';');
            var splits2 = days.Split(';');
            var splits3 = times.Split(';');

            var result = _dataService.GetQuartersCounter(splits1.ToList(), splits2.ToList(), splits3.ToList());
            return new JsonNetResult(result);

        }

        public void UnscheduleSchedulesForSelector(string scheduleIds, string tutorialType){
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();

                var splits = scheduleIds.Split(',').ToList();

                _dataService.UnscheduleSchedulesForSelector(splits, User.Identity.Name);
            }
        }

        public ActionResult GetSchedulesForSelector(string groupCodesAndSpecNames, string tutorialType, string planningType)
        {
            _dataService = new DataService();

            var splits = groupCodesAndSpecNames.Split(',');

            var result = _dataService.GetSchedulesForSelector(splits.ToList(), tutorialType, planningType);

            return new JsonNetResult(result);
        }

        public ActionResult GetPlacingsForSelector(string buildingAndAuditoriumNames, string days, string times, int auditoriumType, string viewType)
        {
            _dataService = new DataService();

            var splits1 = buildingAndAuditoriumNames.Split(';').ToList();
            var splits2 = days.Split(';').ToList();
            var splits3 = times.Split(';').ToList();

            var result = _dataService.GetPlacingsForSelector(splits1, splits2, splits3, auditoriumType, viewType);

            return new JsonNetResult(result);
        }

        public ActionResult GetSchedulesCounterForLecturer(string lecturerName)
        {
            _dataService = new DataService();

            var result = _dataService.GetSchedulesCounterForLecturer(lecturerName);

            return new JsonNetResult(result);
        }

        public ActionResult DellPlanningThreadProfileSelector(string selectorName)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    _dataService = new DataService();
                    _dataService.DellPlanningThreadProfileSelector(selectorName, User.Identity.Name);

                    return new JsonNetResult();
                }
                catch (ProfileSelectorNotFoundException ex)
                {
                    return new JsonNetResult(ex.Message);
                }
            }
            return new JsonNetResult();
        }

        public ActionResult DellPlanningQuartersProfileSelector(string selectorName)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    _dataService = new DataService();
                    _dataService.DellPlanningQuarterProfileSelector(selectorName, User.Identity.Name);

                    return new JsonNetResult();
                }
                catch (ProfileSelectorNotFoundException ex)
                {
                    return new JsonNetResult(ex.Message);
                }
            }
            return new JsonNetResult();
        }

        public ActionResult AddPlanningThreadProfileSelector(string selectorName, string specialityNames, string groupCodes)
        {
            if (User.Identity.IsAuthenticated)
            {              
                try{
                    var splits1 = specialityNames.Split(';').ToList();
                    var splits2 = groupCodes.Split(';').ToList();

                    _dataService = new DataService();
                    _dataService.AddPlanningThreadProfileSelector(selectorName, User.Identity.Name, splits1,splits2);

                    var selector = _dataService.GetPlanningThreadProfile(selectorName, User.Identity.Name);
                    return new JsonNetResult(selector);
                }
                catch(ProfileSelectorAlreadyExsistsException ex){
                    return new JsonNetResult(ex.Message);
                }
                catch (IncorrectProfileSelectorNameException ex)
                {
                    return new JsonNetResult(ex.Message);
                }
            }
            //TODO
            return new JsonNetResult();
        }

        public ActionResult AddPlanningQuartersProfileSelector(string selectorName, string buildingNames, string auditoriumNumbers, string days, string times)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var splits1 = buildingNames.Split(';').ToList();
                    var splits2 = auditoriumNumbers.Split(';').ToList();
                    var splits3 = days.Split(';').ToList();
                    var splits4 = times.Split(';').ToList();

                    _dataService = new DataService();
                    _dataService.AddPlanningQuartersProfileSelector(selectorName, User.Identity.Name, splits1, splits2, splits3, splits4);

                    var selector = _dataService.GetPlanningQuartersProfile(selectorName, User.Identity.Name);
                    return new JsonNetResult(selector);
                }
                catch (ProfileSelectorAlreadyExsistsException ex)
                {
                    return new JsonNetResult(ex.Message);
                }
                catch (IncorrectProfileSelectorNameException ex)
                {
                    return new JsonNetResult(ex.Message);
                }
            }
            //TODO
            return new JsonNetResult();
        }

        public ActionResult GetAllTimes()
        {
            _dataService = new DataService();
            var result = _dataService.GetAllTimes().ToList();

            return new JsonNetResult(result);
        }

        public ActionResult GetGraph()
        {
            _dataService = new DataService();
            var result = _dataService.GetGraph();

            return new JsonNetResult(result);
        }

        public ActionResult GetGraphForSchedules(string groupCodesAndSpecNames, string tutorialType, string planningType)
        {
            _dataService = new DataService();

            if (!String.IsNullOrEmpty(groupCodesAndSpecNames))
            {

                var splits = groupCodesAndSpecNames.Split(',');

                var result = _dataService.GetGraphForSchedules(splits.ToList(), tutorialType, planningType);

                return new JsonNetResult(result);
            }

            return new JsonNetResult();
        }

        public void UnplanningSchedulesForSelector(string scheduleIds)
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();

                var splits0 = scheduleIds.Split(',').ToList();

                _dataService.UnscheduleSchedulesForSelector(splits0, User.Identity.Name);
            }
        }

        public void PlanningSchedulesForSelector(string scheduleIds, string buildingAndAuditoriumNames, string days, string times)
        {
            if (User.Identity.IsAuthenticated)
            {
                _dataService = new DataService();

                var splits0 = scheduleIds.Split(',').ToList();
                var splits1 = buildingAndAuditoriumNames.Split(';').ToList();
                var splits2 = days.Split(';').ToList();
                var splits3 = times.Split(';').ToList();

                _dataService.PlanningSchedulesForSelector(splits0, splits1, splits2, splits3, User.Identity.Name);
            }
        }




    }
}