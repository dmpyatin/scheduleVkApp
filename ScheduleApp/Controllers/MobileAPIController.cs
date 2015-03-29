﻿using ScheduleApp.Infrastructure;
using ScheduleData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using ScheduleData.Models.IAISDataWrappers;
using ScheduleData.Infrastructure;

namespace ScheduleApp.Controllers 
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GroupsSearchController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Group> Get(string template, int count)
        {
            if (count > 15) count = 15;

            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetGroupsByFirstMatching(template, count)
                .Select(x => _dataConverter.GetGroup(x)).ToList();
            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LecturersSearchController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Lecturer> Get(string template, int count)
        {
            if (count > 15) count = 15;

            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetLecturersByFirstMatching(template, count)
                .Select(x => _dataConverter.GetLecturer(x)).ToList();
            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuditoriumsSearchController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Auditorium> Get(string template, int count)
        {
            if (count > 15) count = 15;

            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetAuditoriumsByFirstMatching(template, count)
                .Select(x => _dataConverter.GetAuditorium(x)).ToList();

            return result;
        }
    }


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SchedulesForGroupController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Schedule> Get(string groupCode, string specialityCode)
        {

            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetSchedulesForGroupAPI_Improved(groupCode, specialityCode)
                .Select(x => _dataConverter.GetSchedule(x)).ToList();

            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SchedulesForLecturerController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Schedule> Get(string lecturerName)
        {
            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetSchedulesForLecturer(lecturerName)
                .Select(x => _dataConverter.GetSchedule(x)).ToList();

            return result;
        }
    }


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AgregatedSchedulesForLecturerController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Schedule> Get(string lecturerName)
        {
            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetAgregateSchedulesForLecturer(lecturerName)
                .Select(x => _dataConverter.GetSchedule(x)).ToList();

            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StrongAgregatedSchedulesForLecturerController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Schedule> Get(string lecturerName)
        {
            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetStrongAgregateSchedulesForLecturer(lecturerName)
                .Select(x => _dataConverter.GetSchedule(x)).ToList();

            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SchedulesForAuditoriumController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Schedule> Get(string auditoriumNumber, string buildingShortName)
        {
            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetSchedulesForAuditorium(auditoriumNumber, buildingShortName)
                .Select(x => _dataConverter.GetSchedule(x)).ToList();

            return result;
        }
    }


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AgregatedSchedulesForAuditoriumController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Schedule> Get(string auditoriumNumber, string buildingShortName)
        {
            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetAgregateSchedulesForAuditorium(auditoriumNumber, buildingShortName)
                .Select(x => _dataConverter.GetSchedule(x)).ToList();

            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StrongAgregatedSchedulesForAuditoriumController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Schedule> Get(string auditoriumNumber, string buildingShortName)
        {
            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetStrongAgregateSchedulesForAuditorium(auditoriumNumber, buildingShortName)
                .Select(x => _dataConverter.GetSchedule(x)).ToList();

            return result;
        }
    }


    //TODO
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BuildingsController : ApiController
    {
        DataService _dataService;

        public IList<ScheduleData.Models.Building> Get()
        {
            _dataService = new DataService();
            var result = _dataService.GetAllBuildings(false);

            return result;
        }
    }



    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SchedulesController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Schedule> Get(int count)
        {
            if (count > 15) count = 15;

            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetAllSchedules(count)
                .Select(x => _dataConverter.GetSchedule(x)).ToList();

            return result;
        }
    }

    

    //TODO
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FreeAuditoriumsController : ApiController
    {
        DataService _dataService;

        DataConverter _dataConverter;

        public IList<ScheduleData.Models.IAISDataWrappers.Auditorium> Get(string buildingShortName, int day, string startTime, string endTime, int type, int count)
        {
            if (count > 15) count = 15;

            _dataService = new DataService();
            _dataConverter = new DataConverter();
            var result = _dataService.GetFreeAuditoriums(buildingShortName, day, startTime, endTime, type, count)
                .Select(x => _dataConverter.GetAuditorium(x)).ToList();

            return result;
        }
    }


    


}
