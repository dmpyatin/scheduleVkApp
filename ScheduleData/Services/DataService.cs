using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using ScheduleData.Models;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using System.Configuration;
using System.Text.RegularExpressions;
using ScheduleData.Exceptions;
using ScheduleData.Models.DataTransfer;
using ScheduleData.Models.SigmaGraph;
using ScheduleData.Models.Temp;

namespace ScheduleData.Services
{
    public class DataService
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

        private MongoCollection<PlanningThreadProfileSelector> _threadProfiles;
        private MongoCollection<PlanningQuartersProfileSelector> _quarterProfiles;

        public DataService()
        {
            var con = new MongoConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString);

            MongoClient client = new MongoClient(con.ConnectionString);
            MongoServer server = client.GetServer();

            var db = server.GetDatabase(con.DatabaseName);

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

            _threadProfiles = db.GetCollection<PlanningThreadProfileSelector>("threadprofiles");
            _quarterProfiles = db.GetCollection<PlanningQuartersProfileSelector>("quarterprofiles");
        }

        public IList<Tutorial> GetTutorialsByFirstMatching(string template)
        {
            if (template.Length > 0)
                template = template.First().ToString().ToUpper() + template.Substring(1);

            var query = Query.Or(Query.Matches("Name", "/^" + template.Trim() + "/"),
                                 Query.Matches("ShortName", "/^" + template.Trim() + "/"));

            return _tutorials.Find(query).Take(15).ToList();
        }

        public IList<WeekType> GetWeekTypesByFirstMatching(string template)
        {
            if (template.Length > 0)
                template = template.First().ToString().ToUpper() + template.Substring(1);

            var query = Query.Matches("Name", "/^" + template.Trim() + "/");             
            return _weekTypes.Find(query).Take(15).ToList();
        }


        public IList<Building> GetBuildingsByFirstMatching(string template)
        {
            if (template.Length > 0)
                template = template.First().ToString().ToUpper() + template.Substring(1);

            var query = Query.Or(Query.Matches("Name", "/^" + template.Trim() + "/"),
                                 Query.Matches("ShortName", "/^" + template.Trim() + "/"));

            return _buildings.Find(query).Take(15).ToList();
        }

        public IList<TutorialType> GetTutorialTypesByFirstMatching(string template)
        {
            if (template.Length > 0)
                template = template.First().ToString().ToUpper() + template.Substring(1);

            var query = Query.Matches("Name", "/^" + template.Trim() + "/");

            return _tutorialTypes.Find(query).Take(15).ToList();
        }

        public IList<Speciality> GetSpecialitiesByFirstMatching(string template)
        {
            if (template.Length > 0)
                template = template.First().ToString().ToUpper() + template.Substring(1);

            var query = Query.Or(Query.Matches("Name", "/^" + template.Trim() + "/"),
                                 Query.Matches("Code", "/^" + template.Trim() + "/"));

            return _specialities.Find(query).Take(15).ToList();
        }

        public IList<Auditorium> GetAuditoriumsByFirstMatching(string template, int pair, int day, string weekType, string tutorialType)
        {

            var query = Query.Or(Query.Matches("Number", "/^" + template.Trim() + "/"),
                                 Query.Matches("BuildingShortName", "/^" + template.Trim() + "/"));

            return _auditoriums.Find(query).Take(20).ToList();
        }

        public IList<Time> GetTimesByFirstMatching(string template)
        {

            var query = Query.Or(Query.Matches("StartTime", "/^" + template.Trim() + "/"),
                                 Query.Matches("EndTime", "/^" + template.Trim() + "/"));

            return _times.Find(query).Take(15).ToList();
        }

        public IList<PlanningThreadProfileSelector> GetThreadSelectorsByFirstMatching(string template, string userName)
        {

            var query = Query.And(Query.Matches("Name", "/^" + template + "/"),
                                  Query.EQ("UserName", userName));

            return _threadProfiles.Find(query).Take(15).ToList();
        }

        public IList<PlanningQuartersProfileSelector> GetQuartersSelectorsByFirstMatching(string template, string userName)
        {

            var query = Query.And(Query.Matches("Name", "/^" + template + "/"),
                                  Query.EQ("UserName", userName));

            return _quarterProfiles.Find(query).Take(15).ToList();
        }


        public IList<Lecturer> GetLecturersByFirstMatching(string template)
        {
            if(template.Length > 0)
                template = template.First().ToString().ToUpper() + template.Substring(1);


            var query = Query.Matches("Name", "/^" + template.Trim() + "/");
            return _lecturers.Find(query).Take(15).ToList();
        }

        public IList<ScheduleData.Models.Group> GetGroupsByFirstMatching(string template)
        {
            if (!String.IsNullOrEmpty(template))
            {
                var query = Query.Or(
                    Query.Matches("Code", "/^" + template.Trim() + "/"),
                    Query.Matches("SpecialityName", "/^" + template.Trim() + "/")
                    );
                return _groups.Find(query).Take(15).ToList();
            }
            return new List<ScheduleData.Models.Group>() { };
        }

        public IList<Schedule> GetSchedulesForGroup(string groupCode, string specialityName)
        {
            var sCode = Regex.Match(specialityName, @"\(([^)]*)\)").Groups[1].Value.Trim();
            var query = Query.And(
                Query<Schedule>.Matches(x => x.CurrentVersion.GroupCode, groupCode),
                Query<Schedule>.Matches(x => x.CurrentVersion.SpecialityCode, sCode)
                );
 
            return _schedules.Find(query).ToList();
        }

        public IList<Schedule> GetSchedulesForLecturer(string lecturerName)
        {
            var query = Query<Schedule>.Matches(x => x.CurrentVersion.LecturerName, lecturerName);
            return _schedules.Find(query).ToList();
        }

        public IMongoQuery IsUnscheduledSchedule(string groupCode, string specialityName)
        {
            var sCode = Regex.Match(specialityName, @"\(([^)]*)\)").Groups[1].Value.Trim();

            return Query.And(
                                Query<Schedule>.Matches(x => x.CurrentVersion.GroupCode, groupCode),
                                Query<Schedule>.Matches(x => x.CurrentVersion.SpecialityCode, sCode),
                                Query.Or(Query<Schedule>.EQ(x => x.CurrentVersion.DayOfWeek, 0),
                                         Query<Schedule>.EQ(x => x.CurrentVersion.PairNumber, 0)));
        }

        public IMongoQuery IsUnscheduledSchedule(string groupCode)
        {
            return Query.And(
                                Query<Schedule>.Matches(x => x.CurrentVersion.GroupCode, groupCode),
                                Query.Or(Query<Schedule>.EQ(x => x.CurrentVersion.DayOfWeek, 0),
                                         Query<Schedule>.EQ(x => x.CurrentVersion.PairNumber, 0)));
        }

        public IMongoQuery IsScheduledSchedule(string groupCode)
        {
            return Query.And(
                                Query<Schedule>.Matches(x => x.CurrentVersion.GroupCode, groupCode),
                                Query.And(Query<Schedule>.NE(x => x.CurrentVersion.DayOfWeek, 0),
                                         Query<Schedule>.NE(x => x.CurrentVersion.PairNumber, 0)));
        }

        public IMongoQuery IsScheduledSchedule(string groupCode, string specName)
        {
            return Query.And(
                                Query<Schedule>.EQ(x => x.CurrentVersion.GroupCode, groupCode),
                                Query<Schedule>.EQ(x => x.CurrentVersion.SpecialityName, specName),
                                Query.And(Query<Schedule>.NE(x => x.CurrentVersion.DayOfWeek, 0),
                                         Query<Schedule>.NE(x => x.CurrentVersion.PairNumber, 0)));
        }

        public IMongoQuery IsScheduledScheduleForLecturer(string lecturerName)
        {
            return Query.And(
                                Query<Schedule>.Matches(x => x.CurrentVersion.LecturerName, lecturerName),
                                Query.And(Query<Schedule>.NE(x => x.CurrentVersion.DayOfWeek, 0),
                                         Query<Schedule>.NE(x => x.CurrentVersion.PairNumber, 0)));
        }


        public IList<Schedule> GetUnscheduledSchedulesForGroup(string groupCode, string specialityName)
        {
            return _schedules.Find(IsUnscheduledSchedule(groupCode, specialityName)).ToList();
        }

        public IList<Schedule> GetUnscheduledSchedulesForLecturer(string lecturerName)
        {
            var query = Query.And(
                                Query<Schedule>.Matches(x => x.CurrentVersion.LecturerName, lecturerName),
                                Query.Or(Query<Schedule>.EQ(x => x.CurrentVersion.DayOfWeek, 0),
                                         Query<Schedule>.EQ(x => x.CurrentVersion.PairNumber, 0)));

            return _schedules.Find(query).ToList();
        }

        public IList<Schedule> GetSchedulesForAuditorium(string auditoriumNumber, string buildingShortName)
        {
            var query = Query.And(Query<Schedule>.Matches(x => x.CurrentVersion.AuditoriumNumber, auditoriumNumber),
                                  Query<Schedule>.Matches(x => x.CurrentVersion.BuildingName, buildingShortName));

            return _schedules.Find(query).ToList();
        }

        public IList<Schedule> GetLinkedSchedules(string scheduleId)
        {
            var scheduleQuery = Query.EQ("_id", ObjectId.Parse(scheduleId));
            var schedule = _schedules.FindOne(scheduleQuery);

            var schedules = GetSchedulesForGroup(schedule.CurrentVersion.GroupCode, schedule.CurrentVersion.SpecialityName)
                .Union(GetSchedulesForLecturer(schedule.CurrentVersion.LecturerName)).ToList();


            var result = new List<Schedule>();

            foreach (var s in schedules)
            {
                if (!result.Any(x => x.CurrentVersion.StartTime == s.CurrentVersion.StartTime &&
                               x.CurrentVersion.EndTime == s.CurrentVersion.EndTime && 
                               x.CurrentVersion.WeekTypeName == s.CurrentVersion.WeekTypeName))
                    result.Add(s);
            }


            return result;
        }


        public void UnscheduleSchedule(string scheduleId, string editorName)
        {
            var query = Query.EQ("_id", ObjectId.Parse(scheduleId));

            var update = Update<Schedule>.Set(x => x.CurrentVersion.Editor, editorName).Set(x => x.CurrentVersion.DayOfWeek, 0).Set(x => x.CurrentVersion.PairNumber, 0);
           
            _schedules.FindAndModify(query,SortBy.Null,update);
        }

        public Schedule ChangeSchedule(
                                string scheduleId,
                                string startTime,
                                string endTime,
                                string tutorialName,
                                string weekTypeName,
                                string tutorialTypeName,
                                string auditoriumNumber,
                                string buildingName,
                                string subGroupName,
                                string lecturerName,
                                string editorName)
        {

           

            var scheduleQuery = Query.And(Query.EQ("_id", ObjectId.Parse(scheduleId)));

            var timeQuery = Query.And(Query.EQ("StartTime", startTime), Query.EQ("EndTime", endTime));

            var tutorialQuery = Query.EQ("Name", tutorialName);
            var weekTypeQuery = Query.EQ("Name", weekTypeName);
            var tutorialTypeQuery = Query.EQ("Name", tutorialTypeName);
            var lecturerQuery = Query.EQ("Name", lecturerName);
            var auditoriumQuery = Query.And(Query.EQ("Number", auditoriumNumber), Query.EQ("BuildingShortName", buildingName));
            var buildingQuery = Query.EQ("ShortName",buildingName);

            var time = _times.FindOne(timeQuery);
            var tutorial = _tutorials.FindOne(tutorialQuery);
            var weekType = _weekTypes.FindOne(weekTypeQuery);
            var tutorialType = _tutorialTypes.FindOne(tutorialTypeQuery);
            var lecturer = _lecturers.FindOne(lecturerQuery);
            var auditorium = _auditoriums.FindOne(auditoriumQuery);
            var building = _buildings.FindOne(buildingQuery);

            var schedule = _schedules.FindOne(scheduleQuery);

            //stub
            var updateQuery = Update<Schedule>.Set(x => x.CurrentVersion.SemesterName, schedule.CurrentVersion.SemesterName);

            var edited = false;

            var exception = new EntityNotFoundException();

            if (time != null)
            {
                if(time.StartTime != schedule.CurrentVersion.StartTime ||
                    time.EndTime != schedule.CurrentVersion.EndTime)
                {
                    updateQuery = updateQuery.Set(x => x.CurrentVersion.StartTime, time.StartTime).Set(x => x.CurrentVersion.EndTime, time.EndTime);
                    edited = true;
                }
            }
            else
            {
                exception.Messages.Add("Времени проведения занятия нет в базе данных");
            }

            if(tutorial != null)
            {
                if (tutorial.Name != schedule.CurrentVersion.TutorialName)
                {
                    updateQuery = updateQuery.Set(x => x.CurrentVersion.ThreadName, tutorial.Name);
                    edited = true;
                }
            }
            else
            {
                exception.Messages.Add("Названия предмета нет в базе данных");
            }

            if(weekType != null)
            {
                if (weekType.Name != schedule.CurrentVersion.WeekTypeName)
                {
                    updateQuery = updateQuery.Set(x => x.CurrentVersion.WeekTypeName, weekType.Name);
                    edited = true;
                }
            }
            else
            {
                exception.Messages.Add("Типа недели нет в базе данных");
            }

            if(tutorialType != null)
            {
                if (tutorialType.Name != schedule.CurrentVersion.TutorialTypeName)
                {
                    updateQuery = updateQuery.Set(x => x.CurrentVersion.TutorialTypeName, tutorialType.Name);
                    edited = true;
                }
            }
            else
            {
                exception.Messages.Add("Типа предмета нет в базе данных");
            }

            if(lecturer != null){
                if (lecturer.Name != schedule.CurrentVersion.LecturerName)
                {
                    updateQuery = updateQuery.Set(x => x.CurrentVersion.LecturerName, lecturer.Name);
                    edited = true;
                }
            }
            else
            {
                exception.Messages.Add("Имени преподавателя нет в базе данных");
            }

            if(auditorium != null){
                if (auditorium.Number != schedule.CurrentVersion.AuditoriumNumber)
                {
                    updateQuery = updateQuery.Set(x => x.CurrentVersion.AuditoriumNumber, auditorium.Number);
                    edited = true;
                }
            }
            else
            {
                exception.Messages.Add("Аудитории нет в базе данных");
            }

            //shortName
            if(building != null){
                if (building.ShortName != schedule.CurrentVersion.BuildingName)
                {
                    updateQuery = updateQuery.Set(x => x.CurrentVersion.BuildingName, building.ShortName);
                    edited = true;
                }
            }
            else
            {
                exception.Messages.Add("Названия учебного корпуса нет в базе данных");
            }

            if (exception.Messages.Count > 0)
                throw exception;

            
            if(schedule.CurrentVersion.SubGroupName != subGroupName.Trim()){
                updateQuery = updateQuery.Set(x => x.CurrentVersion.SubGroupName, subGroupName);
                edited = true;
            }

            if(edited == true){
                updateQuery = updateQuery.Set(x => x.CurrentVersion.Editor, editorName);

                _schedules.FindAndModify(scheduleQuery, SortBy.Null, updateQuery);
            }

            schedule = _schedules.FindOne(scheduleQuery);

            return schedule;
        }


        //TODO: Improve
        /*
        public void AddSchedule(string startTime,
                                string endTime,
                                string tutorialName,
                                string weekTypeName,
                                string tutorialTypeName,
                                string lecturerName,
                                string groupCode,
                                string auditoriumNumber,
                                string buildingName,
                                string subGroupName, 
                                string editorName,
                                int pairNumber,
                                int dayOfWeek
                                )
        {

            var timeQuery = Query.And(Query.EQ("StartTime", startTime), Query.EQ("EndTime",endTime));
            var tutorialQuery = Query.EQ("Name", tutorialName);
            var weekTypeQuery = Query.EQ("Name", weekTypeName);
            var tutorialTypeQuery = Query.EQ("Name", tutorialTypeName);
            var lecturerQuery = Query.EQ("Name", lecturerName);
            var groupQuery = Query.EQ("Code", groupCode);
            var auditoriumQuery = Query.And(Query.EQ("Number", auditoriumNumber), Query.EQ("BuildingShortName", buildingName));

            var time = _times.FindOne(timeQuery);
            var tutorial = _tutorials.FindOne(tutorialQuery);
            var weekType = _weekTypes.FindOne(weekTypeQuery);
            var tutorialType = _tutorialTypes.FindOne(tutorialTypeQuery);
            var lecturer = _lecturers.FindOne(lecturerQuery);
            var group = _groups.FindOne(groupQuery);
            var auditorium = _auditoriums.FindOne(auditoriumQuery);

            var exception = new EntityNotFoundException();

            if (time == null)
                exception.Messages.Add("Указанного времени нет в базе данных");

            if (tutorial == null)
                exception.Messages.Add("Указанного названия предмета нет в базе данных");

            if (weekType == null)
                exception.Messages.Add("Указанного типа недели нет в базе данных");

            if (tutorialType == null)
                exception.Messages.Add("Указанного типа предмета нет в базе данных");

            if (lecturer == null)
                exception.Messages.Add("Указанного преподавателя нет в базе данных");

            if (group == null)
                exception.Messages.Add("Указанной группы нет в базе данных");

            if (auditorium == null)
                exception.Messages.Add("Указанной аудитории нет в базе данных");

            if (exception.Messages.Count > 0)
                throw exception;

            var sCode = Regex.Match(group.SpecialityName, @"\(([^)]*)\)").Groups[1].Value.Trim();

            var scheduleVersion = new ScheduleVersion()
            {
                IsShowed = true,
                Version = 1.2,
                Editor = editorName,
                LecturerName = lecturer.Name,
                TutorialName = tutorial.Name,
                TutorialTypeName = tutorialType.Name,
                SubGroupName = subGroupName,
                AuditoriumNumber = auditorium.Number,
                BuildingName = auditorium.BuildingShortName,
                StartTime = time.StartTime,
                EndTime = time.EndTime,
                StartDate = "",
                EndDate = "",
                WeekTypeName = weekType.Name,
                PairNumber = pairNumber,
                DayOfWeek = dayOfWeek,
                StudyYear = "",
                StudyForm = "",
                SpecialityCode = sCode,
                SpecialityName = group.SpecialityName,
                GroupCode = group.Code,
                ThreadName = ""
            };

            var schedule = new Schedule();
            schedule.ScheduleVersions.Add(scheduleVersion);
            schedule.CurrentVersion = schedule.ScheduleVersions[0];

            _schedules.Insert(schedule);

        }*/

        public void SetSchedulePairAndDay(string scheduleId, int pair, int day, string editorName)
        {
            var query = Query.EQ("_id", ObjectId.Parse(scheduleId));

            var sortBy = SortBy.Null;
            var update = Update<Schedule>.Set(x => x.CurrentVersion.Editor, editorName).Set(x => x.CurrentVersion.DayOfWeek, day).Set(x => x.CurrentVersion.PairNumber, pair);

            _schedules.FindAndModify(query, sortBy, update);
        }


        public IList<Schedule> GetAgregateSchedulesForLecturer(string lecturerName)
        {
            var query = Query<Schedule>.Matches(x => x.CurrentVersion.LecturerName, lecturerName);

            var schedules = _schedules.Find(query).ToList();

            var agrSchedules = new List<Schedule>();

            foreach (var s in schedules)
            {
                var sl = agrSchedules.Where(x => x.CurrentVersion.DayOfWeek == s.CurrentVersion.DayOfWeek &&
                                    x.CurrentVersion.WeekTypeName == s.CurrentVersion.WeekTypeName &&
                                    x.CurrentVersion.PairNumber == s.CurrentVersion.PairNumber &&
                                    x.CurrentVersion.TutorialName == s.CurrentVersion.TutorialName &&
                                    x.CurrentVersion.TutorialTypeName == s.CurrentVersion.TutorialTypeName &&
                                    x.CurrentVersion.LecturerName == x.CurrentVersion.LecturerName).ToList();

                if (sl.Count == 0)
                {
                    agrSchedules.Add(s);
                }
                else
                {
                    agrSchedules.Remove(sl[0]);
                    sl[0].CurrentVersion.GroupCode += ", " + s.CurrentVersion.GroupCode;
                    agrSchedules.Add(sl[0]);
                }
                    
            }

            return agrSchedules;
        }


        public IList<Speciality> GetAllSpecialities()
        {
            return _specialities.FindAll().ToList();
        }

        public IList<int> GetAllCourses()
        {
            return new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        }

        public IList<ScheduleData.Models.Group> GetAllGroups(List<string> specialityNames)
        {
            var result = new List<ScheduleData.Models.Group>();

            foreach (var specName in specialityNames)
            {
                var query = Query.EQ("SpecialityName", specName);
                result.AddRange(_groups.Find(query).ToList());
            }

            return result;
        }

        public List<Building> GetAllBuildings()
        {
            return _buildings.FindAll().ToList();
        }

        public List<Auditorium> GetAllAuditoriums(List<string> buildingNames)
        {
            var result = new List<Auditorium>();

            foreach (var buildingName in buildingNames)
            {
                var query = Query.EQ("BuildingShortName", buildingName);
                result.AddRange(_auditoriums.Find(query).ToList());
            }

            return result;
        }

        public PlanningScheduleCounter GetPlanningScheduleCounter(List<Schedule> schedules, List<Schedule> plannedSchedules)
        {
            var lecturesCount = schedules.Where(x => x.CurrentVersion.TutorialTypeName == "Лек").Count();
            var practiceCount = schedules.Where(x => x.CurrentVersion.TutorialTypeName == "Прак").Count();
            var labsCount = schedules.Where(x => x.CurrentVersion.TutorialTypeName == "Лаб").Count();

            var othersCount = schedules.Where(x => x.CurrentVersion.TutorialTypeName != "Лаб" &&
                                                   x.CurrentVersion.TutorialTypeName == "Лек" &&
                                                   x.CurrentVersion.TutorialTypeName == "Прак").Count();


            var plannedLecturesCount = plannedSchedules.Where(x => x.CurrentVersion.TutorialTypeName == "Лек").Count();
            var plannedPracticeCount = plannedSchedules.Where(x => x.CurrentVersion.TutorialTypeName == "Прак").Count();
            var plannedLabsCount = plannedSchedules.Where(x => x.CurrentVersion.TutorialTypeName == "Лаб").Count();

            var plannedOthersCount = plannedSchedules.Where(x => x.CurrentVersion.TutorialTypeName != "Лаб" &&
                                                   x.CurrentVersion.TutorialTypeName == "Лек" &&
                                                   x.CurrentVersion.TutorialTypeName == "Прак").Count();


            var result = new PlanningScheduleCounter(lecturesCount, labsCount, practiceCount, othersCount, plannedLecturesCount, plannedLabsCount, plannedPracticeCount, plannedOthersCount);

            return result;
        }


        public PlanningQuartersCounter GetQuartersCounter(List<string> buildingAndAuditoriumNames, List<string> days, List<string> times)
        {

            var splitedTimes = new List<Tuple<string, string>>();

            foreach (var time in times)
            {
                if (!String.IsNullOrEmpty(time))
                {
                    var splits = time.Split('-');
                    splitedTimes.Add(new Tuple<string, string>(splits[0], splits[1]));
                }
            }

            var auditoriums = new List<Auditorium>();

            foreach (var bldAndAud in buildingAndAuditoriumNames)
            {
                var splits = bldAndAud.Split('#');

                var query = Query.And(Query<Auditorium>.EQ(x => x.Number, splits[0]),
                                      Query<Auditorium>.EQ(x => x.BuildingShortName, splits[1]));

                auditoriums.AddRange(_auditoriums.Find(query).ToList());
            }

            var allAuditoriumsCount = auditoriums.Where(x => x.Type == 1).Count()*times.Count*days.Count;
            var allCabinetsCount = auditoriums.Where(x => x.Type == 2).Count() * times.Count * days.Count;
            var allLaboratoriesCount = auditoriums.Where(x => x.Type == 4).Count() * times.Count * days.Count;
            var allDisplayRoomsCount = auditoriums.Where(x => x.Type == 3).Count() * times.Count * days.Count;
            var allLingafonRoomsCount = auditoriums.Where(x => x.Type == 8).Count() * times.Count * days.Count;
            var allHallsCount = auditoriums.Where(x => x.Type == 6).Count() * times.Count * days.Count;
            var allOthersCount = auditoriums.Where(x => x.Type != 1 &&
                                                        x.Type != 2 &&
                                                        x.Type != 3 &&
                                                        x.Type != 4 &&
                                                        x.Type != 6 &&
                                                        x.Type != 8).Count() * times.Count * days.Count;

            var allAllCount = auditoriums.Count * times.Count * days.Count;

            var schedules = _schedules.FindAll().ToList().Where(x => splitedTimes.Any(y => y.Item1 == x.CurrentVersion.StartTime &&
                                                                                           y.Item2 == x.CurrentVersion.EndTime) &&
                                                                     days.Any(y => y == x.CurrentVersion.DayOfWeek.ToString())).ToList();

            var busyAuditoriumsCount = auditoriums.Where(x => x.Type == 1 && schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).Count()*times.Count*days.Count;
            var busyCabinetsCount = auditoriums.Where(x => x.Type == 2 && schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).Count() * times.Count * days.Count;
            var busyLaboratoriesCount = auditoriums.Where(x => x.Type == 4 && schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).Count() * times.Count * days.Count;
            var busyDisplayRoomsCount = auditoriums.Where(x => x.Type == 3 && schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).Count() * times.Count * days.Count;
            var busyLingafonRoomsCount = auditoriums.Where(x => x.Type == 8 && schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).Count() * times.Count * days.Count;
            var busyHallsCount = auditoriums.Where(x => x.Type == 6 && schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).Count() * times.Count * days.Count;
            var busyOthersCount = auditoriums.Where(x => x.Type != 1 &&
                                                        x.Type != 2 &&
                                                        x.Type != 3 &&
                                                        x.Type != 4 &&
                                                        x.Type != 6 &&
                                                        x.Type != 8 && schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).Count() * times.Count * days.Count;

            var busyAllCount = auditoriums.Where(x => schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).Count() * times.Count * days.Count;

            return new PlanningQuartersCounter(
                allAuditoriumsCount,
                allCabinetsCount,
                allLaboratoriesCount,
                allDisplayRoomsCount,
                allLingafonRoomsCount,                             
                allHallsCount,
                allOthersCount,
                allAllCount,
                busyAuditoriumsCount,
                busyCabinetsCount,
                busyLaboratoriesCount,
                busyDisplayRoomsCount,
                busyLingafonRoomsCount,
                busyHallsCount,
                busyOthersCount,
                busyAllCount);
        }


        public PlanningScheduleCounter GetSchedulesCounterForGroups(List<string> groupCodesAndSpecNames)
        {
            var schedules = new List<Schedule>();
            var plannedSchedules = new List<Schedule>();

            foreach (var grCodeAndSpecName in groupCodesAndSpecNames)
            {
                var splits = grCodeAndSpecName.Split('#');

                var query = Query.And(Query.EQ("CurrentVersion.GroupCode", splits[0]),
                                      Query.EQ("CurrentVersion.SpecialityName", splits[1]));

                schedules.AddRange(_schedules.Find(query).ToList());
                plannedSchedules.AddRange(_schedules.Find(IsScheduledSchedule(splits[0],splits[1])).ToList());
            }

            return GetPlanningScheduleCounter(schedules, plannedSchedules);
            
        }

        public PlanningScheduleCounter GetSchedulesCounterForLecturer(string lecturerName)
        {
            var schedules = new List<Schedule>();
            var plannedSchedules = new List<Schedule>();

            var query = Query.EQ("CurrentVersion.LecturerName", lecturerName);
            schedules = _schedules.Find(query).ToList();

            plannedSchedules = _schedules.Find(IsScheduledScheduleForLecturer(lecturerName)).ToList();
           
            return GetPlanningScheduleCounter(schedules, plannedSchedules);
        }

       
        public void AddPlanningThreadProfileSelector(string name, string userName, List<string> specialityNames, List<string> groupCodes){

            if (String.IsNullOrEmpty(name))
            {
                throw new IncorrectProfileSelectorNameException("Недопустимое имя селектора");
            }
        
            var profile = new PlanningThreadProfileSelector(name, userName, specialityNames, groupCodes);

            var query = Query.And(Query<PlanningThreadProfileSelector>.EQ(x => x.Name, name),
                                  Query<PlanningThreadProfileSelector>.EQ(x => x.UserName, userName));

            if (_threadProfiles.Find(query).Count() > 0)
            {
                throw new ProfileSelectorAlreadyExsistsException("Селектор с данным именем уже существует");
            }

            _threadProfiles.Insert(profile);
        }

        public void AddPlanningQuartersProfileSelector(string name, string userName, List<string> buildingNames, List<string> auditoriumNumbers, List<string> days, List<string> times)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new IncorrectProfileSelectorNameException("Недопустимое имя селектора");
            }

            var profile = new PlanningQuartersProfileSelector(name, userName, buildingNames, auditoriumNumbers, days, times);

            var query = Query.And(Query<PlanningQuartersProfileSelector>.EQ(x => x.Name, name),
                                  Query<PlanningQuartersProfileSelector>.EQ(x => x.UserName, userName));

            if (_quarterProfiles.Find(query).Count() > 0)
            {
                throw new ProfileSelectorAlreadyExsistsException("Селектор с данным именем уже существует");
            }
                
            _quarterProfiles.Insert(profile);          
        }

        public void DellPlanningThreadProfileSelector(string name, string userName)
        {
            var query = Query.And(Query<PlanningThreadProfileSelector>.EQ(x => x.Name, name),
                                  Query<PlanningThreadProfileSelector>.EQ(x => x.UserName, userName));

            if (_threadProfiles.Find(query).Count() == 0)
            {
                throw new ProfileSelectorNotFoundException("Селектор с данным именем не найден");
            }

            _threadProfiles.Remove(query);
        }

        public void DellPlanningQuarterProfileSelector(string name, string userName)
        {
            var query = Query.And(Query<PlanningQuartersProfileSelector>.EQ(x => x.Name, name),
                                  Query<PlanningQuartersProfileSelector>.EQ(x => x.UserName, userName));

            if (_quarterProfiles.Find(query).Count() == 0)
            {
                throw new ProfileSelectorNotFoundException("Селектор с данным именем не найден");
            }

            _quarterProfiles.Remove(query);
        }

        public List<PlanningThreadProfileSelector> GetAllThreadProfileSelectors(string userName)
        {
            var query = Query<PlanningThreadProfileSelector>.EQ(x => x.UserName, userName);

            var result = _threadProfiles.Find(query).ToList();

            return result;
        }

        public List<PlanningQuartersProfileSelector> GetAllQuartersProfileSelectors(string userName)
        {
            var query = Query<PlanningQuartersProfileSelector>.EQ(x => x.UserName, userName);

            var result = _quarterProfiles.Find(query).ToList();

            return result;
        }


        public PlanningThreadProfile GetPlanningThreadProfile(string profileSelectorName, string userName)
        {
            var query = Query.And(Query<PlanningThreadProfileSelector>.EQ(x => x.Name, profileSelectorName),
                                  Query<PlanningThreadProfileSelector>.EQ(x => x.UserName, userName));

            var selector = _threadProfiles.Find(query).FirstOrDefault();

            var specialities = _specialities.FindAll().ToList().Where(x => selector.SpecialityNames.Any(y => y == x.Name)).ToList();
            var groups = _groups.FindAll().ToList().Where(x => selector.GroupCodes.Any(y => y == x.Code)).ToList();

            var profile = new PlanningThreadProfile(specialities,groups);

            return profile;
        }

        public PlanningQuartersProfile GetPlanningQuartersProfile(string profileSelectorName, string userName)
        {
            var query = Query.And(Query<PlanningQuartersProfileSelector>.EQ(x => x.Name, profileSelectorName),
                                  Query<PlanningQuartersProfileSelector>.EQ(x => x.UserName, userName));

            var selector = _quarterProfiles.Find(query).FirstOrDefault();

            var buildings = _buildings.FindAll().ToList().Where(x => selector.BuildingNames.Any(y => y == x.ShortName)).ToList();
            var auditoriums = _auditoriums.FindAll().ToList().Where(x => selector.AuditoriumNumbers.Any(y => y == x.Number)).ToList();

            var days = selector.Days;


            var splitedTimes = new List<Tuple<string, string>>();

            foreach (var time in selector.Times)
            {
                if (!String.IsNullOrEmpty(time))
                {
                    var splits = time.Split('-');
                    splitedTimes.Add(new Tuple<string, string>(splits[0], splits[1]));
                }
            }

            var times = _times.FindAll().ToList().Where(x => splitedTimes.Any(y => y.Item1 == x.StartTime && y.Item2 == x.EndTime)).ToList();

            var profile = new PlanningQuartersProfile(buildings, auditoriums,days,times);

            return profile;
        }


        public List<Time> GetAllTimes()
        {
            return _times.FindAll().ToList();
        }

        public List<Placing> GetPlacingsForSelector(List<string> buildingAndAuditoriumNames, List<string> days, List<string> times, int auditoriumType, string viewType)
        {
            var result = new List<Placing>();

            var auditoriums = new List<Auditorium>();

            var splitedTimes = new List<Tuple<string, string>>();

            foreach (var time in times)
            {
                if (!String.IsNullOrEmpty(time))
                {
                    var splits = time.Split('-');
                    splitedTimes.Add(new Tuple<string, string>(splits[0], splits[1]));
                }
            }

            var times2 = _times.FindAll().ToList().Where(x => splitedTimes.Any(y => y.Item1 == x.StartTime && y.Item2 == x.EndTime)).ToList();

            foreach (var bld in buildingAndAuditoriumNames)
            {
                var splits = bld.Split('#');

                var query = Query.And(Query<Auditorium>.EQ(x => x.BuildingShortName, splits[1]),
                                      Query<Auditorium>.EQ(x => x.Number, splits[0]));

                auditoriums.AddRange(_auditoriums.Find(query).ToList());
            }

            if (auditoriumType != -1 && auditoriumType != -2)
            {
                auditoriums = auditoriums.Where(x => x.Type == auditoriumType).ToList();
            }
            else
            {
                if (auditoriumType == -1)
                {
                    auditoriums = auditoriums.Where(x => x.Type != 1 &&
                                                            x.Type != 2 &&
                                                            x.Type != 3 &&
                                                            x.Type != 4 &&
                                                            x.Type != 6 &&
                                                            x.Type != 8).ToList();
                }
            }

            if (viewType == "all")
            {
                foreach (var auditorium in auditoriums)
                {
                    foreach (var day in days)
                    {
                        foreach (var time in times2)
                        {
                            result.Add(new Placing(time, int.Parse(day), auditorium));
                        }
                    }
                }

                return result;
            }

            if (viewType == "busy")
            {
                var schedules = _schedules.FindAll().ToList().Where(x => splitedTimes.Any(y => y.Item1 == x.CurrentVersion.StartTime &&
                                                                                           y.Item2 == x.CurrentVersion.EndTime) &&
                                                                     days.Any(y => y == x.CurrentVersion.DayOfWeek.ToString())).ToList();

                auditoriums = auditoriums.Where(x => schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).ToList();

                foreach (var auditorium in auditoriums)
                {
                    foreach (var day in days)
                    {
                        foreach (var time in times2)
                        {
                            result.Add(new Placing(time, int.Parse(day), auditorium));
                        }
                    }
                }

                return result;
            }

            if (viewType == "free")
            {
                var schedules = _schedules.FindAll().ToList().Where(x => splitedTimes.Any(y => y.Item1 == x.CurrentVersion.StartTime &&
                                                                                           y.Item2 == x.CurrentVersion.EndTime) &&
                                                                     days.Any(y => y == x.CurrentVersion.DayOfWeek.ToString())).ToList();

                auditoriums = auditoriums.Where(x => !schedules.Any(y => y.CurrentVersion.AuditoriumNumber == x.Number)).ToList();

                foreach (var auditorium in auditoriums)
                {
                    foreach (var day in days)
                    {
                        foreach (var time in times2)
                        {
                            result.Add(new Placing(time, int.Parse(day), auditorium));
                        }
                    }
                }

                return result;
            }

            return result;
        }


        public void PlanningSchedulesForSelector(List<string> scheduleIds, List<string> buildingAndAuditoriumNames, List<string> days, List<string> times, string editorName)
        {
            var placings = GetPlacingsForSelector(buildingAndAuditoriumNames, days, times, -2, "free");
  
            var bsonScheduleIds = scheduleIds.Select(x => ObjectId.Parse(x)).ToList();

            if (placings.Count >= scheduleIds.Count)
            {
                foreach (var scheduleId in scheduleIds)
                {
                    var randomPlacing = placings.First();

                    var query = Query.EQ("_id", ObjectId.Parse(scheduleId));

                    var timeToPair = new TimeToPair();

                    var sortBy = SortBy.Null;

                    var pair = timeToPair.GetPair(randomPlacing.Time);

                    var update = Update<Schedule>.Set(x => x.CurrentVersion.Editor, editorName)
                        .Set(x => x.CurrentVersion.DayOfWeek, randomPlacing.Day)
                        .Set(x => x.CurrentVersion.StartTime, randomPlacing.Time.StartTime)
                        .Set(x => x.CurrentVersion.EndTime, randomPlacing.Time.EndTime)
                        .Set(x => x.CurrentVersion.PairNumber, pair)
                        .Set(x => x.CurrentVersion.AuditoriumNumber, randomPlacing.Auditorium.Number);

                    _schedules.FindAndModify(query, sortBy, update);
                   
                    placings.Remove(randomPlacing);
                }
            }

        }

        public void UnscheduleSchedulesForSelector(List<string> scheduleIds, string editorName)
        {
            foreach (var scheduleId in scheduleIds)
            {
                UnscheduleSchedule(scheduleId, editorName);
            }
        }
        

        public List<Schedule> GetSchedulesForSelector(List<string> groupCodesAndSpecNames, string tutorialType, string planningType){
            var result = new List<Schedule>();

            var schedules = new List<Schedule>();
            var plannedSchedules = new List<Schedule>();
            var unplannedSchedules = new List<Schedule>();

            foreach (var grCodeAndSpecName in groupCodesAndSpecNames)
            {
                var splits = grCodeAndSpecName.Split('#');

                var query = Query.And(Query.EQ("CurrentVersion.GroupCode", splits[0]),
                                      Query.EQ("CurrentVersion.SpecialityName", splits[1]));

                schedules.AddRange(_schedules.Find(query).ToList());
                plannedSchedules.AddRange(_schedules.Find(IsScheduledSchedule(splits[0], splits[1])).ToList());
                unplannedSchedules.AddRange(_schedules.Find(IsUnscheduledSchedule(splits[0], splits[1])).ToList());
            }


            if (planningType == "all")
            {
                if (tutorialType != "all")
                {
                    if (tutorialType != "other")
                    {
                        return schedules.Where(x => x.CurrentVersion.TutorialTypeName == tutorialType).ToList();
                    }
                    return schedules.Where(x => x.CurrentVersion.TutorialTypeName != "Лек" &&
                                         x.CurrentVersion.TutorialTypeName != "Лаб" &&
                                         x.CurrentVersion.TutorialTypeName != "Прак").ToList();
                }
                return schedules;
            }

            if (planningType == "planned")
            {
                if (tutorialType != "all")
                {
                    if (tutorialType != "other")
                    {
                        return plannedSchedules.Where(x => x.CurrentVersion.TutorialTypeName == tutorialType).ToList();
                    }

                    return plannedSchedules.Where(x => x.CurrentVersion.TutorialTypeName != "Лек" &&
                                         x.CurrentVersion.TutorialTypeName != "Лаб" &&
                                         x.CurrentVersion.TutorialTypeName != "Прак").ToList();
                }
                return plannedSchedules;
            }

            if (planningType == "unplanned")
            {
                if (tutorialType != "all")
                {
                    if (tutorialType != "other")
                    {
                        return unplannedSchedules.Where(x => x.CurrentVersion.TutorialTypeName == tutorialType).ToList();
                    }
                    return unplannedSchedules.Where(x => x.CurrentVersion.TutorialTypeName != "Лек" &&
                                         x.CurrentVersion.TutorialTypeName != "Лаб" &&
                                         x.CurrentVersion.TutorialTypeName != "Прак").ToList();
                }
                return unplannedSchedules;
            }

            return result;
        }


        public Graph GetGraph()
        {
            var result = new Graph();

            result.AddNode(new Node("n0", "A Node", 0, 0, 3));
            result.AddNode(new Node("n1", "Another", 3, 1, 2));

            result.AddEdge(new Edge("e1", "n0", "n1"));

            return result;
        }

        public Graph GetGraphForSchedules(List<string> groupCodesAndSpecNames, string tutorialType, string planningType)
        {
            var result = new Graph();

            var schedules = new List<Schedule>();
            var plannedSchedules = new List<Schedule>();
            var unplannedSchedules = new List<Schedule>();

            foreach (var grCodeAndSpecName in groupCodesAndSpecNames)
            {
                var splits = grCodeAndSpecName.Split('#');

                var query = Query.And(Query.EQ("CurrentVersion.GroupCode", splits[0]),
                                      Query.EQ("CurrentVersion.SpecialityName", splits[1]));

                schedules.AddRange(_schedules.Find(query).ToList());
                plannedSchedules.AddRange(_schedules.Find(IsScheduledSchedule(splits[0], splits[1])).ToList());
                unplannedSchedules.AddRange(_schedules.Find(IsUnscheduledSchedule(splits[0], splits[1])).ToList());
            }

            int curX = 0, curY = 0, nodeId = 0, edgeId = 0;

            var nodeMap = new Dictionary<string, string>();

            foreach (var schedule in schedules)
            {
               
                var secondNodeLabel = schedule.CurrentVersion.AuditoriumNumber + " " + schedule.CurrentVersion.BuildingName;

                var secondNodeId = "n" + (nodeId + 1).ToString();

                if (!nodeMap.Any(x => x.Value == secondNodeLabel))
                {
                    nodeMap.Add(secondNodeId, secondNodeLabel);
                    result.AddNode(new Node(secondNodeId, secondNodeLabel, curX + 10, curY, 0.1));
                }
                else
                {
                    secondNodeId = nodeMap.Where(x => x.Value == secondNodeLabel).FirstOrDefault().Key;
                }

                result.AddNode(new Node("n" + nodeId.ToString(), schedule.CurrentVersion.TutorialName, curX, curY, 0.1));
               
                result.AddEdge(new Edge("e" + edgeId.ToString(), "n" + nodeId.ToString(), secondNodeId));
                //curX++;
                curY++;
                nodeId += 2;
                edgeId += 1;
            }


            return result;
        }

    }
}
