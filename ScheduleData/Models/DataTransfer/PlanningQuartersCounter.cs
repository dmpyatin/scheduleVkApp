using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.DataTransfer
{
    public class PlanningQuartersCounter
    {
        public int AllAuditoriumsCount { get; set; }
        public int AllCabinetsCount { get; set; }
        public int AllLaboratoriesCount { get; set; }
        public int AllDisplayRoomsCount {get; set;}
        public int AllLingafonRoomsCount {get; set;}
        public int AllOthersCount {get; set;}
        public int AllHallsCount {get; set;}
        public int AllAllCount { get; set; }

        public int FreeAuditoriumsCount {get; set;}
        public int FreeCabinetsCount {get; set;}
        public int FreeLaboratoriesCount {get; set;}
        public int FreeDisplayRoomsCount {get; set;}
        public int FreeLingafonRoomsCount {get; set;}
        public int FreeOthersCount {get; set;}
        public int FreeHallsCount {get; set;}
        public int FreeAllCount { get; set; }

        public int BusyAuditoriumsCount {get; set;}
        public int BusyCabinetsCount {get; set;}
        public int BusyLaboratoriesCount {get; set;}
        public int BusyDisplayRoomsCount {get; set;}
        public int BusyLingafonRoomsCount {get; set;}
        public int BusyOthersCount {get; set;}
        public int BusyHallsCount {get; set;}
        public int BusyAllCount { get; set; }

        public PlanningQuartersCounter(int allAuditoriumsCount,
                                       int allCabinetsCount,
                                       int allLaboratoriesCount,
                                       int allDisplayRoomsCount,
                                       int allLingafonRoomsCount,
                                       int allHallsCount,
                                       int allOthersCount,
                                       int allAllCount,
                                       int busyAuditoriumsCount,
                                       int busyCabinetsCount,
                                       int busyLaboratoriesCount,
                                       int busyDisplayRoomsCount,
                                       int busyLingafonRoomsCount,
                                       int busyHallsCount,
                                       int busyOthersCount,
                                       int busyAllCount)
        {
            AllAuditoriumsCount = allAuditoriumsCount;
            AllCabinetsCount = allCabinetsCount;
            AllLaboratoriesCount = allLaboratoriesCount;
            AllDisplayRoomsCount = allDisplayRoomsCount;
            AllLingafonRoomsCount = allLingafonRoomsCount;
            AllHallsCount = allHallsCount;
            AllOthersCount = allOthersCount;
            AllAllCount = allAllCount;
            BusyAuditoriumsCount = busyAuditoriumsCount;
            BusyCabinetsCount = busyCabinetsCount;
            BusyLaboratoriesCount = busyLaboratoriesCount;
            BusyDisplayRoomsCount = busyDisplayRoomsCount;
            BusyLingafonRoomsCount = busyLingafonRoomsCount;
            BusyHallsCount = busyHallsCount;
            BusyOthersCount = busyOthersCount;
            BusyAllCount = busyAllCount;
            FreeAuditoriumsCount = AllAuditoriumsCount - BusyAuditoriumsCount;
            FreeCabinetsCount = AllCabinetsCount - BusyCabinetsCount;
            FreeLaboratoriesCount = AllLaboratoriesCount - BusyLaboratoriesCount;
            FreeDisplayRoomsCount = AllDisplayRoomsCount - BusyDisplayRoomsCount;
            FreeLingafonRoomsCount = AllLingafonRoomsCount - BusyLingafonRoomsCount;
            FreeHallsCount = AllHallsCount - BusyHallsCount;
            FreeOthersCount = AllOthersCount - BusyOthersCount;
            FreeAllCount = AllAllCount - BusyAllCount;
        }
    }
}
