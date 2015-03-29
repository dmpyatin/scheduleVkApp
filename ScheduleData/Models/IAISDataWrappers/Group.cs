﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.IAISDataWrappers
{
    public class Group
    {
        public string Id { get; set; }

        public string Code { get; set; }
        public string Course { get; set; }
        public string FacultyName { get; set; }
        public string SpecialityName { get; set; }
        public string SpecialityCode { get; set; }
    }

    /*
 * in: template string, count int
 SELECT * FROM
 SCHEDULE_GROUPS_VIEW
 WHERE
 CODE LIKE template || '%' AND
 ROWNUM <= count AND
 ROWNUM <= 15;
 * /


//short SCHEDULE_GROUPS_VIEW
/*
SELECT 
V_STUD_GR.UBU_ID AS Id,
V_STUD_GR.GR_CODE AS Code,
V_STUD_GR.KURS_CODE AS Course, 
V_STUD_GR.NAME_FACULT AS FacultyName,
V_STUD_GR.NAME_SPEC AS SpecialityName, 
V_STUD_GR.SPEC_CODE AS SpecialityCode
FROM 
V_RASP_DESK_N, V_STUD_GR
WHERE
V_RASP_DESK_N.UBU_UBU_ID = V_STUD_GR.UBU_ID*/

}