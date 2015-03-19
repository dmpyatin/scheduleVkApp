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

        // Controller methods not shown...
        public IList<ScheduleData.Models.Group> Get(string template)
        {
            _dataService = new DataService();
            var result = _dataService.GetGroupsByFirstMatching(template);
            return result;
        }

    }
}
