using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.IAISDataWrappers
{
    public class Lecturer
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }
    }
}


/*
 * in: template string, count int
 * 
 * 
SELECT 
*
FROM
SCHEDULE_LECTURERS_WITHOUT_ID_VIEW
WHERE
F_NAME IS NOT NULL AND
I_NAME IS NOT NULL AND
O_NAME IS NOT NULL)
WHERE LASTNAME LIKE 'Куз%' AND
WHERE LASTNAME LIKE  template || '%' AND
ROWNUM <= count AND
ROWNUM <= 15;
 */

//SCHEDULE_LECTURERS_WITHOUT_ID_VIEW
/*
SELECT 
DISTINCT
F_NAME AS Lastname,
I_NAME AS Firstname,
O_NAME AS Secondname,
F_NAME || ' ' || SUBSTR(I_NAME,0,1) || '. ' || SUBSTR(O_NAME,0,1) || '.' AS FullName
FROM 
V_RASP_DESK_N
WHERE
F_NAME IS NOT NULL AND
I_NAME IS NOT NULL AND
O_NAME IS NOT NULL
*/


//SCHEDULE_LECTURERS_WITH_ID_VIEW
/*
SELECT 
DISTINCT
WPL_ID AS Id,
F_NAME AS Lastname,
I_NAME AS Firstname,
O_NAME AS Secondname,
F_NAME || ' ' || SUBSTR(I_NAME,0,1) || '. ' || SUBSTR(O_NAME,0,1) || '.' AS FullName
FROM 
V_RASP_DESK_N
WHERE
F_NAME IS NOT NULL AND
I_NAME IS NOT NULL AND
O_NAME IS NOT NULL
*/
