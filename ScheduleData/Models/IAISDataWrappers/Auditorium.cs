using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.IAISDataWrappers
{

    public class Auditorium
    {
        public string Id { get; set; }

        public string Num { get; set; }
        public string BuildingShortName { get; set; }
        public int Building { get; set; }
        public string ShortName { get; set; }
        public string IAIS_ID { get; set; }
    }

    /*
     * in: template string, count int
     SELECT * FROM
     AUDITORIUMS_VIEW
     WHERE
     (NUM LIKE template || '%' OR BUILDINGSHORTNAME LIKE template || '%') AND
     ROWNUM <= count AND
     ROWNUM <= 15;
     * /


        //AUDITORIUMS_VIEW
        /*
        SELECT
            B_QUARTERS.ID AS Id,
            B_QUARTERS.CODE AS Num,
            B_BULDINGS.NAMESHORT AS BuildingShortName,
            B_QUARTERS.BLD_ID AS Building,
            B_QUARTERS.NAMESHORT AS ShortName,
            B_QUARTERS.ID AS IAIS_ID
            FROM
                 B_BULDINGS, 
                 B_QUARTERS
            WHERE   
                (B_QUARTERS.BLD_ID = B_BULDINGS.ID AND
                 B_QUARTERS.LIVING = 'N' AND
                 B_BULDINGS.STATUS = 'Y' AND
                 B_QUARTERS.STATUS = 'Y')*/
}

