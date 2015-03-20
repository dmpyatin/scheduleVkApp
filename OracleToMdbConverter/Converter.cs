using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using MongoDB.Bson;
using ScheduleData.Models;
using System.Configuration;
using Oracle.DataAccess.Client;

namespace OracleToMdbConverter
{
    public class Converter
    {
        private MongoCollection<Schedule> _schedules;
        private MongoCollection<ScheduleData.Models.Group> _groups;
        private MongoCollection<Lecturer> _lecturers;
        private MongoCollection<Speciality> _specialities;
        private MongoCollection<Building> _buildings;
        private MongoCollection<Tutorial> _tutorials;
        private MongoCollection<TutorialType> _tutorialTypes;
        private MongoCollection<Auditorium> _auditoriums;
        private MongoCollection<Time> _times;
        private MongoCollection<WeekType> _weekTypes;

        private List<Building> _orBuildings;
        private List<ScheduleData.Models.Group> _orGroups;
        private List<Lecturer> _orLecturers;
        private List<Speciality> _orSpecialities;
        private List<Schedule> _orSchedules;
        private List<Tutorial> _orTutorials;
        private List<TutorialType> _orTutorialTypes;
        private List<Auditorium> _orAuditoriums;
        private List<Time> _orTimes;
        private List<WeekType> _orWeekTypes;

        private OracleConnection oracleConn;

        public Converter()
        {
            var mdbConn = new MongoConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString);

            MongoClient client = new MongoClient(mdbConn.ConnectionString);
            MongoServer server = client.GetServer();

            var db = server.GetDatabase(mdbConn.DatabaseName);
            _schedules = db.GetCollection<Schedule>("schedules");
            _groups = db.GetCollection<ScheduleData.Models.Group>("groups");
            _lecturers = db.GetCollection<Lecturer>("lecturers");
            _specialities = db.GetCollection<Speciality>("specialities");
            _buildings = db.GetCollection<Building>("buildings");
            _auditoriums = db.GetCollection<Auditorium>("auditoriums");
            _tutorials = db.GetCollection<Tutorial>("tutorials");
            _tutorialTypes = db.GetCollection<TutorialType>("tutorialtypes");
            _times = db.GetCollection<Time>("times");
            _weekTypes = db.GetCollection<WeekType>("weektypes");

            _orBuildings = new List<Building>();
            _orGroups = new List<ScheduleData.Models.Group>();
            _orAuditoriums = new List<Auditorium>();
            _orLecturers = new List<Lecturer>();
            _orSchedules = new List<Schedule>();
            _orSpecialities = new List<Speciality>();
            _orTimes = new List<Time>();
            _orTutorials = new List<Tutorial>();
            _orTutorialTypes = new List<TutorialType>();
            _orWeekTypes = new List<WeekType>();


            oracleConn = new OracleConnection(ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString);

        }


        void LoadBuildings(bool withLiving)
        {
            oracleConn.Open();

            var cmd = oracleConn.CreateCommand();

            if (!withLiving)
            {
                cmd.CommandText = @"SELECT        
                                    ID AS IAIS_ID, 
                                    NAMEFULL AS Name, 
                                    NAMESHORT AS ShortName,
                                    ADDRESS AS Address
                                FROM            
                                    SDMS.B_BULDINGS
                                WHERE        
                                    (STATUS = 'Y' AND 
                                     TYPE_HOST IS NULL)";
            }
            else
            {
                cmd.CommandText = @"SELECT        
                                    ID AS IAIS_ID, 
                                    NAMEFULL AS Name, 
                                    NAMESHORT AS ShortName,
                                    ADDRESS AS Address
                                FROM            
                                    SDMS.B_BULDINGS
                                WHERE        
                                    (STATUS = 'Y')";
            }
            var reader = cmd.ExecuteReader();

 
            while (reader.Read())
            {

                var IAIS_Id = reader.GetInt64(0);
                var Name = reader.GetString(1);
                var ShortName = reader.GetString(2);


                var Address = "";

                if (!reader.IsDBNull(3))
                {
                    Address = reader.GetString(3);
                }

                var building = new Building()
                {
                    IAIS_ID = IAIS_Id.ToString(),
                    Name = Name,
                    ShortName = ShortName,
                    Address = Address
                };

                //_buildings.Insert(building);
                _orBuildings.Add(building);
            }

            reader.Dispose();
            cmd.Dispose();

            oracleConn.Close();
            oracleConn.Dispose();
        }

        void LoadAuditoriums()
        {
            oracleConn.Open();

            var cmd = oracleConn.CreateCommand();
            cmd.CommandText = @"SELECT
                                     B_QUARTERS.CODE AS Number,
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
                                      B_QUARTERS.STATUS = 'Y')";

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var Number = reader.GetString(0);
                var BuildingShortName = reader.GetString(1);
                var Building = reader.GetInt64(2);
                var ShortName = reader.GetString(3);
                var IAIS_Id = reader.GetInt64(4);

                var auditorium = new Auditorium()
                {
                    Number = Number,
                    BuildingShortName = BuildingShortName,
                    Building = (int)Building,
                    ShortName = ShortName,
                    IAIS_ID = IAIS_Id.ToString()
                };

                _orAuditoriums.Add(auditorium);
            }

            reader.Dispose();
            cmd.Dispose();

            oracleConn.Close();
            oracleConn.Dispose();
        }

        void LoadSpecialities()
        {
            oracleConn.Open();

            var cmd = oracleConn.CreateCommand();
            cmd.CommandText = @"SELECT 
	                                DISTINCT 
	                                V_STUD_GR.SPEC_CODE AS Code,
	                                V_STUD_GR.NAME_SPEC AS Name,
                                    V_STUD_GR.SPEC_BUN_ID AS IAIS_ID
                                FROM
	                                V_RASP_DESK_N,
	                                V_STUD_GR,
	                                U_RASP_STR
                                WHERE
	                                (V_RASP_DESK_N.SR_ID = U_RASP_STR.SR_ID AND
	                                U_RASP_STR.UBU_UBU_ID = V_STUD_GR.UBU_ID)";

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var Code = reader.GetString(0);
                var Name = reader.GetString(1);
                var IAIS_Id = reader.GetInt64(2);
   
                var speciality = new Speciality()
                {
                    Code = Code,
                    Name = Name,
                    IAIS_ID = IAIS_Id.ToString()                 
                };

                _orSpecialities.Add(speciality);
            }

            reader.Dispose();
            cmd.Dispose();

            oracleConn.Close();
            oracleConn.Dispose();
        }

        public void Load(string tableName)
        {
            if (tableName == "buildings")
            {
                this.LoadBuildings(false);
            }

            if (tableName == "auditoriums")
            {
                this.LoadAuditoriums();
            }

            if (tableName == "specialities")
            {
                this.LoadSpecialities();
            }
        }

        public void SaveAll()
        {

        }



        //Building ShortName is the main linked parameter
        public void SyncBuildings(bool removePrevious)
        {
            if (_orBuildings.Count == 0)
            {
                this.LoadBuildings(true);
            }

            if (removePrevious)
            {
                _buildings.RemoveAll();
            }

            foreach (var building in _orBuildings)
            {
                var query = Query<Building>.EQ(x => x.ShortName, building.ShortName);
                var currentBuildings = _buildings.Find(query).ToList();


                if (String.IsNullOrEmpty(building.Address))
                    building.Address = building.Name + " ПетрГУ г. Петрозаводск";

                if (currentBuildings.Count == 0)
                {
                    
                    _buildings.Insert(building);
                }
                else
                {

                    var update = Update<Building>
                        .Set(x => x.IAIS_ID, building.IAIS_ID)
                        .Set(x => x.Name, building.Name)
                        .Set(x => x.ShortName, building.ShortName)
                        .Set(x => x.Address, building.Address);

                    _buildings.FindAndModify(query, SortBy.Null, update);
                }


            }

            var auditoriums = _auditoriums.FindAll().ToList();

            foreach (var auditorium in auditoriums)
            {
                var query = Query<Building>.EQ(x => x.ShortName, auditorium.BuildingShortName);
                var currentBuilding = _buildings.Find(query).ToList().First();

                var newID = int.Parse(currentBuilding.IAIS_ID);

                var update = Update<Auditorium>.Set(x => x.Building, newID);

                var audUpdateQuery = Query<Auditorium>.EQ(x => x.Id, auditorium.Id);

                _auditoriums.FindAndModify(audUpdateQuery, SortBy.Null, update);
            }


            var schedules = _schedules.FindAll().ToList();
            foreach (var schedule in schedules)
            {
                var query = Query<Building>.EQ(x => x.ShortName, schedule.CurrentVersion.BuildingName);
                var currentBuildings = _buildings.Find(query).ToList();

                if (currentBuildings.Count > 0)
                {
                    var currentBuilding = currentBuildings.First();

                    //it must update scheudle versions array too
                    var update = Update<Schedule>.Set(x => x.CurrentVersion.BuildingAddress, currentBuilding.Address);


                    var scheduleUpdateQuery = Query<Schedule>.EQ(x => x.Id, schedule.Id);

                    _schedules.FindAndModify(scheduleUpdateQuery, SortBy.Null, update);
                }            
            }

        }


    }
}
