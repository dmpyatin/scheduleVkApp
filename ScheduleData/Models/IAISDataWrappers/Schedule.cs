using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.IAISDataWrappers
{
    public class Schedule
    {
        public string id { get; set; }
        public string auditoriumid { get; set; }
        public string lecturerid { get; set; }
        public string groupid { get; set; }

        public string fullname { get; set; }
        public string fullnames { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string secondname { get; set; }

        public string tutorialname { get; set; }
        public string tutorialtypename { get; set; }

        public string subgroupname { get; set; }

        public string auditoriumnumber { get; set; }
        public string buildingname { get; set; }
        public string buildingaddress { get; set; }

        public string starttime { get; set; }
        public string endtime { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }

        public string weektypename { get; set; }
        public int pairnumber { get; set; }
        public int dayofweek { get; set; }

        public string studyyear { get; set; }
        public string semestername { get; set; }
        public string studyform { get; set; }
        public string specialitycode { get; set; }
        public string specialityname { get; set; }
        public string groupcode { get; set; }

        public int course { get; set; }

    }
}

/*
 * in: auditoriumNumber string, buildingShortName string
SELECT 

  MAX(LecturerId) AS LecturerId,
  MAX(AuditoriumId) AS AuditoriumId,
  MAX(GroupId) AS GroupId,
  
  MAX(Firstname) AS Firstname,
  MAX(Secondname) AS Secondname,
  MAX(Lastname) AS Lastname,
  LISTAGG(SubGroupName, ', ')  WITHIN GROUP (ORDER BY SubGroupName) AS SubGroupName,
  MAX(AuditoriumName) AS AuditoriumName,
  
  MAX(BuildingName) AS BuildingName,
  MAX(BuildingAddress) AS BuildingAddress,
  MAX(StartDate) AS StartDate,
  MAX(EndDate) AS EndDate,
  MAX(StudyYear) AS StudyYear,
  MAX(Semester) AS Semester,
  MAX(StudyForm) AS StudyForm,
  
  LISTAGG(SpecialityCode, ', ') WITHIN GROUP (ORDER BY SpecialityCode) AS SpecialityCode,
  LISTAGG(SpecialityName, ', ') WITHIN GROUP (ORDER BY SpecialityName) AS SpecialityName,
  LISTAGG(GroupCode, ', ') WITHIN GROUP (ORDER BY GroupCode) AS GroupCode,
  LISTAGG(Course, ', ') WITHIN GROUP (ORDER BY Course) AS Course,
  
  STARTTIME, 
  ENDTIME, 
  WEEKTYPENAME, 
  PAIRNUMBER, 
  FULLNAME, 
  TUTORIALNAME,
  AUDITORIUMNUMBER

FROM

SCHEDULE_VIEW

WHERE
  AUDITORIUMNUMBER = auditoriumNumber AND
  BUILDINGSHORTNAME = buildingShortName

GROUP BY
  STARTTIME, 
  ENDTIME, 
  WEEKTYPENAME, 
  PAIRNUMBER, 
  FULLNAME, 
  TUTORIALNAME,
  AUDITORIUMNUMBER

*/

/*
 * in: lecturerFullName string
SELECT 

  MAX(LecturerId) AS LecturerId,
  MAX(AuditoriumId) AS AuditoriumId,
  MAX(GroupId) AS GroupId,
  
  MAX(Firstname) AS Firstname,
  MAX(Secondname) AS Secondname,
  MAX(Lastname) AS Lastname,
  LISTAGG(SubGroupName, ', ')  WITHIN GROUP (ORDER BY SubGroupName) AS SubGroupName,
  MAX(AuditoriumName) AS AuditoriumName,
  
  MAX(BuildingName) AS BuildingName,
  MAX(BuildingAddress) AS BuildingAddress,
  MAX(StartDate) AS StartDate,
  MAX(EndDate) AS EndDate,
  MAX(StudyYear) AS StudyYear,
  MAX(Semester) AS Semester,
  MAX(StudyForm) AS StudyForm,
  
  LISTAGG(SpecialityCode, ', ') WITHIN GROUP (ORDER BY SpecialityCode) AS SpecialityCode,
  LISTAGG(SpecialityName, ', ') WITHIN GROUP (ORDER BY SpecialityName) AS SpecialityName,
  LISTAGG(GroupCode, ', ') WITHIN GROUP (ORDER BY GroupCode) AS GroupCode,
  LISTAGG(Course, ', ') WITHIN GROUP (ORDER BY Course) AS Course,
  
  STARTTIME, 
  ENDTIME, 
  WEEKTYPENAME, 
  PAIRNUMBER, 
  FULLNAME, 
  TUTORIALNAME,
  AUDITORIUMNUMBER

FROM

SCHEDULE_VIEW

WHERE
  FULLNAME = lecturerFullName

GROUP BY
  STARTTIME, 
  ENDTIME, 
  WEEKTYPENAME, 
  PAIRNUMBER, 
  FULLNAME, 
  TUTORIALNAME,
  AUDITORIUMNUMBER

*/

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


