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
                Building = auditorium.Building,
                Num = auditorium.Number,
                BuildingShortName = auditorium.BuildingShortName,
                ShortName = auditorium.ShortName,

            };

            return res;
        }


        public ScheduleData.Models.IAISDataWrappers.Lecturer GetLecturer(ScheduleData.Models.Lecturer lecturer)
        {
            var res = new ScheduleData.Models.IAISDataWrappers.Lecturer()
            {
                FullName = lecturer.Name
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
                Code = group.Code,
                Course = group.Course.ToString(),
                SpecialityName = specialityName,
                SpecialityCode = specialityCode
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
                Id = schedule.Id.ToString(),
                LecturerFullName = schedule.CurrentVersion.LecturerName,
                TutorialName = schedule.CurrentVersion.TutorialName,
                TutorialTypeName = schedule.CurrentVersion.TutorialTypeName,
                SubGroupName = schedule.CurrentVersion.SubGroupName,

                AuditoriumNumber = schedule.CurrentVersion.AuditoriumNumber,
                BuildingName = schedule.CurrentVersion.BuildingName,
                BuildingAddress = schedule.CurrentVersion.BuildingAddress,

                StartTime = schedule.CurrentVersion.StartTime,
                EndTime = schedule.CurrentVersion.EndTime,

                StartDate = schedule.CurrentVersion.StartDate,
                EndDate = schedule.CurrentVersion.EndDate,

                WeekTypeName = schedule.CurrentVersion.WeekTypeName,
                PairNumber = schedule.CurrentVersion.PairNumber,
                DayOfWeek = schedule.CurrentVersion.DayOfWeek,


                StudyYear = schedule.CurrentVersion.StudyYear,
                SemesterName = schedule.CurrentVersion.SemesterName,
                StudyForm = schedule.CurrentVersion.StudyForm,
                SpecialityCode = specialityCode,

                SpecialityName = specialityName,
                GroupCode = schedule.CurrentVersion.GroupCode,
                Course = schedule.CurrentVersion.Course
            };

            return res;
        }
    }
}
