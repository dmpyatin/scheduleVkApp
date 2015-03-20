using ScheduleApp.Infrastructure;
using ScheduleData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace ScheduleApp.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GroupsSearchController : ApiController
    {
        DataService _dataService;

        public IList<ScheduleData.Models.Group> Get(string template, int count)
        {
            if (count > 15) count = 15;

            _dataService = new DataService();
            var result = _dataService.GetGroupsByFirstMatching(template, count);
            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LecturersSearchController : ApiController
    {
        DataService _dataService;

        public IList<ScheduleData.Models.Lecturer> Get(string template, int count)
        {
            if (count > 15) count = 15;

            _dataService = new DataService();
            var result = _dataService.GetLecturersByFirstMatching(template, count);
            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuditoriumsSearchController : ApiController
    {
        DataService _dataService;

        public IList<ScheduleData.Models.Auditorium> Get(string template, int count)
        {
            if (count > 15) count = 15;

            _dataService = new DataService();
            var result = _dataService.GetAuditoriumsByFirstMatching(template, count);

            return result;
        }
    }


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SchedulesForGroupController : ApiController
    {
        DataService _dataService;

        public IList<ScheduleData.Models.Schedule> Get(string groupCode, string specialityName)
        {

            _dataService = new DataService();
            var result = _dataService.GetSchedulesForGroup(groupCode, specialityName);

            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SchedulesForLecturerController : ApiController
    {
        DataService _dataService;

        public IList<ScheduleData.Models.Schedule> Get(string lecturerName)
        {
            _dataService = new DataService();
            var result = _dataService.GetAgregateSchedulesForLecturer(lecturerName);

            return result;
        }
    }



    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BuildingsController : ApiController
    {
        DataService _dataService;

        public IList<ScheduleData.Models.Building> Get()
        {
            _dataService = new DataService();
            var result = _dataService.GetAllBuildings();

            return result;
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SchedulesController : ApiController
    {
        DataService _dataService;

        public IList<ScheduleData.Models.Schedule> Get(int count)
        {
            if (count > 15) count = 15;

            _dataService = new DataService();
            var result = _dataService.GetAllSchedules(count);

            return result;
        }
    }


}
