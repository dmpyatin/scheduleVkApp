using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ScheduleData.Models
{
    public class ScheduleVersion
    {
        public bool IsShowed { get; set; }
        public double Version { get; set; }
        public string Editor { get; set; }

        public string LecturerName { get; set; }
        public string TutorialName { get; set; }
        public string TutorialTypeName { get; set; }

        public string SubGroupName { get; set; }
        public string AuditoriumNumber { get; set; }
        public string BuildingName { get; set; }
        public string BuildingAddress { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string WeekTypeName { get; set; }
        public int PairNumber { get; set; }
        public int DayOfWeek { get; set; }

        public string StudyYear { get; set; }
        public string SemesterName { get; set; }
        public string StudyForm { get; set; }
        public string SpecialityCode { get; set; }
        public string SpecialityName { get; set; }
        public string GroupCode { get; set; }
        public string ThreadName { get; set; }

        public int Course { get; set; }

        public string IAIS_ID { get; set; }
    }

    public class Schedule
    {
        public ObjectId Id { get; set; }
        public IList<ScheduleVersion> ScheduleVersions { get; set; }
        public ScheduleVersion CurrentVersion { get; set; }

        public Schedule()
        {
            ScheduleVersions = new List<ScheduleVersion>();
        }
    }
}
