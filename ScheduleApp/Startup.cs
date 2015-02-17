using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(ScheduleApp.Startup))]

[assembly: OwinStartupAttribute(typeof(ScheduleApp.Startup))]
namespace ScheduleApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
