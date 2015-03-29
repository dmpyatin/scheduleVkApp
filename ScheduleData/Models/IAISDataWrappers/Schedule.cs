using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.IAISDataWrappers
{
    public class Schedule
    {
        public string Id { get; set; }
        public string AuditoriumId { get; set; }
        public string LecturerId { get; set; }
        public string GroupId { get; set; }

        public string LecturerFullName { get; set; }
        public string LecturerFirstName { get; set; }
        public string LecturerLastName { get; set; }
        public string LecturerSecondName { get; set; }

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

        public int Course { get; set; }

    }
}



/*
 * in: lecturerName string
SELECT * FROM SCHEDULE_VIEW
WHERE
	LECTURERFULLNAME = lecturerName
GROUP_BY

 
 
 
in: lecturerName string
SELECT * FROM SCHEDULE_VIEW
WHERE
	FULLNAME = lecturerName
 
in: auditoriumNumber string, buildingShortNameString
SELECT * FROM SCHEUDLE_VIEW
WHERE
	AUDITORIUMNUMBER = auditoriumNumber AND
	BUILDINGSHORTNAME = buildingShortName
	
in: groupCode string, specCode string
SELECT * FROM SCHEDULE_VIEW
WHERE
	GROUPCODE = groupCode AND
	SPECIALITYCODE = specCode


in: auditoriumId
SELECT * FROM SCHEDULE_VIEW
WHERE
	AUDITORIUMID = auditoriumId

in: groupId
SELECT * FROM SCHEDULE_VIEW
WHERE
	GROUPID = groupId
*/


/* SCHEDULE_VIEW
SELECT
  V_RASP_DESK_N.WPL_ID AS LecturerId,
  V_RASP_DESK_N.BQR_ID AS AuditoriumId,
  V_RASP_DESK_N.UBU_UBU_ID AS GroupId,

	V_RASP_DESK_N.F_NAME || ' ' || SUBSTR(V_RASP_DESK_N.I_NAME,0,1) || '. ' || SUBSTR(V_RASP_DESK_N.O_NAME,0,1) || '.' AS FullName,
	V_RASP_DESK_N.F_NAME AS Lastname,
	V_RASP_DESK_N.I_NAME AS Firstname,
	V_RASP_DESK_N.O_NAME AS Secondname,
	
	V_RASP_DESK_N.NAME AS TutorialName,
	V_RASP_DESK_N.ABBREV AS TutorialTypeName,

	V_RASP_DESK_N.COMMENTARY AS SubGroupName,

	V_BUILD_QUART.CODE AS AuditoriumNumber,
	V_BUILD_QUART.Q_NAMESHORT AS AuditoriumName,
	B_BULDINGS.NAMESHORT AS BuildingName,
	B_BULDINGS.ADDRESS AS BuildingAddress,

	V_RASP_DESK_N.TIME_FROM AS StartTime,
	V_RASP_DESK_N.TIME_TO AS EndTime,
	V_RASP_DESK_N.DATE_FROM AS StartDate,
	V_RASP_DESK_N.DATE_TO AS EndDate,

	V_RASP_DESK_N.PERIOD AS WeekTypeName,
	V_RASP_DESK_N.COD_PAR AS PairNumber,
	V_RASP_DESK_N.UDW_CODE AS DayOfWeek,

	V_RASP_DESK_N.UCH_GOD AS StudyYear,	
	V_RASP_DESK_N.SEM AS Semester,

	V_RASP_DESK_N.FO AS StudyForm,
	
	V_STUD_GR.SPEC_CODE AS SpecialityCode,
	V_STUD_GR.NAME_SPEC AS SpecialityName,

	V_STUD_GR.GR_CODE AS GroupCode,

	V_RASP_DESK_N.KURS AS Course

FROM
V_RASP_DESK_N, V_STUD_GR, V_BUILD_QUART, B_BULDINGS
WHERE
V_RASP_DESK_N.UBU_UBU_ID = V_STUD_GR.UBU_ID
AND B_BULDINGS.ID = V_RASP_DESK_N.BLD_ID
AND (V_RASP_DESK_N.BQR_ID = V_BUILD_QUART.Q_ID(+))
*/


