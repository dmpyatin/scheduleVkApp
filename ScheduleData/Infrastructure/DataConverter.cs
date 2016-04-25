using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Infrastructure
{
    public class DataConverter
    {
        public ScheduleData.Models.IAISDataWrappers.Auditorium GetAuditorium(ScheduleData.Models.Auditorium auditorium)
        {
            var res = new ScheduleData.Models.IAISDataWrappers.Auditorium()
            {
                building = auditorium.Building,
                num = auditorium.Number,
                buildingshortname = auditorium.BuildingShortName,
                shortname = auditorium.ShortName,

            };

            return res;
        }


        public ScheduleData.Models.IAISDataWrappers.Lecturer GetLecturer(ScheduleData.Models.Lecturer lecturer)
        {
            var res = new ScheduleData.Models.IAISDataWrappers.Lecturer()
            {
                fullname = lecturer.Name,
                fullnames = lecturer.Name
            };

            return res;
        }


        public ScheduleData.Models.IAISDataWrappers.Group GetGroup(ScheduleData.Models.Group group)
        {

            var specialityFullName = group.SpecialityName;

            var splits = specialityFullName.Split('(');

            var specialityName = splits[0];
            var specialityCode = splits[1];

            specialityName = specialityName.Remove(specialityName.Length - 1);
            specialityCode = specialityCode.Remove(specialityCode.Length - 1);

            var res = new ScheduleData.Models.IAISDataWrappers.Group()
            {
                code = group.Code,
                course = group.Course.ToString(),
                specialityname = specialityName,
                specialitycode = specialityCode
            };

            return res;
        }

        public ScheduleData.Models.IAISDataWrappers.Schedule GetSchedule(ScheduleData.Models.Schedule schedule)
        {

            var specialityFullName = schedule.CurrentVersion.SpecialityName;

            var splits = specialityFullName.Split('(');

            var specialityName = splits[0];
            var specialityCode = splits[1];

            specialityName = specialityName.Remove(specialityName.Length - 1);
            specialityCode = specialityCode.Remove(specialityCode.Length - 1);

            var res = new ScheduleData.Models.IAISDataWrappers.Schedule()
            {
                id = schedule.Id.ToString(),
                fullname = schedule.CurrentVersion.LecturerName,
                tutorialname = schedule.CurrentVersion.TutorialName,
                tutorialtypename = schedule.CurrentVersion.TutorialTypeName,
                subgroupname = schedule.CurrentVersion.SubGroupName,

                auditoriumnumber = schedule.CurrentVersion.AuditoriumNumber,
                buildingname = schedule.CurrentVersion.BuildingName,
                buildingaddress = schedule.CurrentVersion.BuildingAddress,

                starttime = schedule.CurrentVersion.StartTime,
                endtime = schedule.CurrentVersion.EndTime,

                startdate = schedule.CurrentVersion.StartDate,
                enddate = schedule.CurrentVersion.EndDate,

                weektypename = schedule.CurrentVersion.WeekTypeName,
                pairnumber = schedule.CurrentVersion.PairNumber,
                dayofweek = schedule.CurrentVersion.DayOfWeek,


                studyyear = schedule.CurrentVersion.StudyYear,
                semestername = schedule.CurrentVersion.SemesterName,
                studyform = schedule.CurrentVersion.StudyForm,
                specialitycode = specialityCode,

                specialityname = specialityName,
                groupcode = schedule.CurrentVersion.GroupCode,
                course = schedule.CurrentVersion.Course
            };

            return res;
        }
    }
}
