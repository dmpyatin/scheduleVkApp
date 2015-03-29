using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleData.Models;

namespace OracleToMdbConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var converter = new Converter();
            /*converter.Load("buildings");
            converter.Load("auditoriums");
            converter.Load("specialities");
            converter.SaveAll();*/

            converter.SyncBuildings(true);
        }
    }
}
