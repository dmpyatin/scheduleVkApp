using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.DataTransfer
{
    public class PlanningScheduleCounter
    {
        public int LecturesCount { get; set; }
        public int LabsCount { get; set; }
        public int PracticeCount { get; set; }
        public int OthersCount { get; set; }
        public int TotalCount { get; set; }

        public int PlannedLecturesCount { get; set; }
        public int PlannedLabsCount { get; set; }
        public int PlannedPracticeCount { get; set; }
        public int PlannedOthersCount { get; set; }
        public int PlannedTotalCount { get; set; }

        public int UnplannedLecturesCount { get; set; }
        public int UnplannedLabsCount { get; set; }
        public int UnplannedPracticeCount { get; set; }
        public int UnplannedOthersCount { get; set; }
        public int UnplannedTotalCount { get; set; }

        public PlanningScheduleCounter(int lecturesCount, int labsCount, int practiceCount, int othersCount)
        {
            LecturesCount = lecturesCount;
            LabsCount = labsCount;
            PracticeCount = practiceCount;
            OthersCount = othersCount;
            TotalCount = LecturesCount + LabsCount + PracticeCount + OthersCount;      
        }

        public PlanningScheduleCounter(int lecturesCount, int labsCount, int practiceCount, int othersCount,
                                       int plannedLecturesCount, int plannedLabsCount, int plannedPracticeCount, int plannedOthersCount)
        {
            LecturesCount = lecturesCount;
            LabsCount = labsCount;
            PracticeCount = practiceCount;
            OthersCount = othersCount;
            TotalCount = LecturesCount + LabsCount + PracticeCount + OthersCount;

            PlannedLecturesCount = plannedLecturesCount;
            PlannedLabsCount = plannedLabsCount;
            PlannedPracticeCount = plannedPracticeCount;
            PlannedOthersCount = plannedOthersCount;
            PlannedTotalCount = PlannedLecturesCount + PlannedLabsCount + PlannedPracticeCount + PlannedOthersCount;

            UnplannedLecturesCount = lecturesCount-plannedLecturesCount;
            UnplannedLabsCount = labsCount-plannedLabsCount;
            UnplannedPracticeCount = practiceCount-plannedPracticeCount;
            UnplannedOthersCount = othersCount-plannedOthersCount;
            UnplannedTotalCount = UnplannedLecturesCount + UnplannedLabsCount + UnplannedPracticeCount + UnplannedOthersCount;
        }
    }
}
