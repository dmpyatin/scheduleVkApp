using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleData.Models;

using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using System.Configuration;

namespace SeleniumPetrSU
{
    public class MdbMigrator
    {
        private MongoCollection<Building> _buildingsSource, _buildingsTarget;
        private MongoCollection<Tutorial> _tutorialsSource, _tutorialsTarget;
        private MongoCollection<TutorialType> _tutorialTypesSource, _tutorialTypesTarget;
        private MongoCollection<Auditorium> _auditoriumsSource, _auditoriumsTarget;
        private MongoCollection<Time> _timesSource, _timesTarget;
        private MongoCollection<WeekType> _weekTypesSource, _weekTypesTarget;


        public MdbMigrator()
        {
            var conSource = new MongoConnectionStringBuilder(ConfigurationManager.ConnectionStrings["MongoDbSource"].ConnectionString);
            var conTarget = new MongoConnectionStringBuilder(ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString);

            MongoClient clientSource = new MongoClient(conSource.ConnectionString);
            MongoServer serverSource = clientSource.GetServer();

            var dbSource = serverSource.GetDatabase(conSource.DatabaseName);

            MongoClient clientTarget = new MongoClient(conTarget.ConnectionString);
            MongoServer serverTarget = clientTarget.GetServer();

            var dbTarget = serverTarget.GetDatabase(conTarget.DatabaseName);


            _buildingsSource = dbSource.GetCollection<Building>("buildings");
            _auditoriumsSource = dbSource.GetCollection<Auditorium>("auditoriums");
            _tutorialsSource = dbSource.GetCollection<Tutorial>("tutorials");
            _tutorialTypesSource = dbSource.GetCollection<TutorialType>("tutorialtypes");
            _timesSource = dbSource.GetCollection<Time>("times");
            _weekTypesSource = dbSource.GetCollection<WeekType>("weektypes");


            _buildingsTarget = dbTarget.GetCollection<Building>("buildings");
            _auditoriumsTarget = dbTarget.GetCollection<Auditorium>("auditoriums");
            _tutorialsTarget = dbTarget.GetCollection<Tutorial>("tutorials");
            _tutorialTypesTarget = dbTarget.GetCollection<TutorialType>("tutorialtypes");
            _timesTarget = dbTarget.GetCollection<Time>("times");
            _weekTypesTarget = dbTarget.GetCollection<WeekType>("weektypes");

        }

        public void Migrate()
        {
            var buildings = _buildingsSource.FindAll().ToList();
            var auditoriums = _auditoriumsSource.FindAll().ToList();
            var tutorials = _tutorialsSource.FindAll().ToList();
            var tutorialTypes = _tutorialTypesSource.FindAll().ToList();
            var times = _timesSource.FindAll().ToList();
            var weekTypes = _weekTypesSource.FindAll().ToList();

            foreach (var building in buildings)
            {
                _buildingsTarget.Insert(building);
            }

            foreach (var auditorium in auditoriums)
            {
                _auditoriumsTarget.Insert(auditorium);
            }

            foreach (var tutorial in tutorials)
            {
                _tutorialsTarget.Insert(tutorial);
            }

            foreach (var tutorialType in tutorialTypes)
            {
                _tutorialTypesTarget.Insert(tutorialType);
            }

            foreach (var time in times)
            {
                _timesTarget.Insert(time);
            }

            foreach (var weekType in weekTypes)
            {
                _weekTypesTarget.Insert(weekType);
            }

        }
    }
}
