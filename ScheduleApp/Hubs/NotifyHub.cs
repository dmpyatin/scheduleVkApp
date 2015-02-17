using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ScheduleApp.Hubs
{
    public class NotifyHub : Hub
    {

        public void ChangeScheduleNotify(string scheduleId)
        {
            Clients.Others.callScheduleNotify(scheduleId);
        }

        public void ChangeSchedulesNotify(string scheduleIds)
        {
            Clients.Others.callScheduleNotify(scheduleIds);
        }

    }
}