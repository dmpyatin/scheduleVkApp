using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ScheduleData.Models;
using System.Text.RegularExpressions;

using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using System.Configuration;

namespace SeleniumPetrSU
{
    public class ScheduleGrabber
    {

        public void Grab()
        {
            var con = new MongoConnectionStringBuilder(ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString);

            MongoClient client = new MongoClient(con.ConnectionString);
            MongoServer server = client.GetServer();
            var database = server.GetDatabase(con.DatabaseName);

            var schedules = database.GetCollection<Schedule>("schedules");
            var lecturers = database.GetCollection<Lecturer>("lecturers");

            var specialities = database.GetCollection<Speciality>("specialities");
            var groups = database.GetCollection<ScheduleData.Models.Group>("groups");


            var dayOfWeek = new List<string>() { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };
            var dayCodes = new List<string>() { "R49876625019637550", "R50076414636879223", "R50078421562881213", "R50080426757882698", "R50082633337884604", "R50084808534886830" };
            var specialitiesRows = 196;


            using (var driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("http://student.iias.petrsu.ru/pls/apex/f?p=185:1:1014510470805265");

                var studyYear = (new SelectElement(driver.FindElement(By.Id("P1_UCH_GOD")))).SelectedOption.Text;
                var studyForm = (new SelectElement(driver.FindElement(By.Id("P1_FO")))).SelectedOption.Text;
                var semesterName = (new SelectElement(driver.FindElement(By.Id("P1_SEMESTR")))).SelectedOption.Text;

                Console.WriteLine(String.Format("{0} {1} {2}", studyYear, studyForm, semesterName));

                for (int j = 0; j < specialitiesRows; ++j)
                {
                    var specHolder = driver.FindElementsByXPath("//*[@id='P1_SPEC_holder']/tbody/tr/td[2]/a/img");
                    string existingWindowHandle = driver.CurrentWindowHandle;
                    specHolder[0].Click();
                    Thread.Sleep(1500);
                    //get the current window handles 
                    string popupHandle = string.Empty;
                    ReadOnlyCollection<string> windowHandles = driver.WindowHandles;



                    foreach (string handle in windowHandles)
                    {
                        if (handle != existingWindowHandle)
                        {
                            popupHandle = handle; break;
                        }
                    }
                    //switch to new window 
                    driver.SwitchTo().Window(popupHandle);
                    var specialityElement = driver.FindElementByXPath(String.Format("/html/body/form/div[2]/a[{0}]", j + 1));

                    var specialityUnparsedName = specialityElement.Text.Trim();
                    var specCode = Regex.Match(specialityUnparsedName, @"\(([^)]*)\)").Groups[1].Value.Trim();

                    long scount = specialities.FindAs<Speciality>(Query<Speciality>.EQ(x => x.Code, specCode)).Count();
                    if (scount == 0)
                    {
                        specialities.Insert(new Speciality() { Code = specCode, Name = specialityUnparsedName });
                    }




                    Console.WriteLine(specialityUnparsedName);

                    specialityElement.Click();


                    //driver.Close();
                    driver.SwitchTo().Window(existingWindowHandle);
                    var courseSelect = new SelectElement(driver.FindElement(By.Id("P1_KURS")));

                    var m = courseSelect.Options.Count;

                    for (int k = 0; k < m; ++k)
                    {
                        courseSelect = new SelectElement(driver.FindElement(By.Id("P1_KURS")));
                        courseSelect.SelectByIndex(k);
                        var courseVal = new SelectElement(driver.FindElement(By.Id("P1_KURS"))).SelectedOption.Text.Trim();

                        var groupSelect = new SelectElement(driver.FindElement(By.Id("P1_GR")));
                        var n = groupSelect.Options.Count;
                        for (int i = 0; i < n; ++i)
                        {
                            groupSelect = new SelectElement(driver.FindElement(By.Id("P1_GR")));
                            groupSelect.SelectByIndex(i);

                            var groupCode = (new SelectElement(driver.FindElement(By.Id("P1_GR")))).SelectedOption.Text.Trim();

                            var query = Query.And(
                                Query<ScheduleData.Models.Group>.EQ(x => x.Code, groupCode),
                                Query<ScheduleData.Models.Group>.EQ(x => x.SpecialityName, specialityUnparsedName));

                            long gcount = specialities.FindAs<ScheduleData.Models.Group>(query).Count();
                            if (gcount == 0)
                            {
                                groups.Insert(new ScheduleData.Models.Group() { Code = groupCode, SpecialityName = specialityUnparsedName, Course = int.Parse(courseVal.Trim()) });
                            }


                            Console.WriteLine(groupCode);

                            var goButton = driver.FindElementsByXPath("//*[@id='apex_layout_49852409604245175']/tbody/tr[2]/td[7]/a");
                            goButton[0].Click();
                            Thread.Sleep(200);


                            for (int dc = 0; dc < dayCodes.Count; ++dc)
                            {
                                //Console.WriteLine(dayOfWeek[dc]);
                                var rowsCount = driver.FindElementsByXPath(String.Format("//*[@id='{0}']/tbody/tr", dayCodes[dc])).Count;

                                if (rowsCount > 1)
                                {
                                    for (int rc = 0; rc < rowsCount - 2; ++rc)
                                    {
                                        var pairNumber = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[1] ", dayCodes[dc], rc + 2)).Text;
                                        var weekTypeName = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[2] ", dayCodes[dc], rc + 2)).Text;
                                        var startTime = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[3] ", dayCodes[dc], rc + 2)).Text;
                                        var endTime = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[4] ", dayCodes[dc], rc + 2)).Text;
                                        var tutorialName = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[5] ", dayCodes[dc], rc + 2)).Text;
                                        var lecturerName = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[6] ", dayCodes[dc], rc + 2)).Text;
                                        var tutorialTypeName = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[7] ", dayCodes[dc], rc + 2)).Text;
                                        var subGroupName = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[8] ", dayCodes[dc], rc + 2)).Text;
                                        var auditoriumNumber = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[9] ", dayCodes[dc], rc + 2)).Text;
                                        var buildingName = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[10] ", dayCodes[dc], rc + 2)).Text;
                                        var startDate = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[11] ", dayCodes[dc], rc + 2)).Text;
                                        var endDate = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[12] ", dayCodes[dc], rc + 2)).Text;
                                        var threadName = driver.FindElementByXPath(String.Format("//*[@id='{0}']/tbody/tr[{1}]/td[13] ", dayCodes[dc], rc + 2)).Text;


                                        var sv = new ScheduleVersion()
                                        {
                                            IsShowed = true,
                                            Version = 1.1,
                                            Editor = "",
                                            LecturerName = lecturerName.Trim(),
                                            TutorialName = tutorialName.Trim(),
                                            TutorialTypeName = tutorialTypeName.Trim(),
                                            SubGroupName = subGroupName.Trim(),
                                            AuditoriumNumber = auditoriumNumber.Trim(),
                                            BuildingName = buildingName.Trim(),
                                            StartTime = startTime.Trim(),
                                            EndTime = endTime.Trim(),
                                            StartDate = startDate.Trim(),
                                            EndDate = endDate.Trim(),
                                            WeekTypeName = weekTypeName.Trim(),
                                            PairNumber = int.Parse(pairNumber),
                                            DayOfWeek = dc + 1,

                                            StudyYear = studyYear.Trim(),
                                            SemesterName = semesterName.Trim(),
                                            StudyForm = studyForm.Trim(),
                                            SpecialityCode = specCode.Trim(),
                                            SpecialityName = specialityUnparsedName.Trim(),
                                            GroupCode = groupCode.Trim(),
                                            ThreadName = threadName.Trim(),

                                            Course = int.Parse(courseVal.Trim())
                                        };


                                        var s = new Schedule();
                                        s.ScheduleVersions.Add(sv);
                                        s.CurrentVersion = sv;

                                        schedules.Insert(s);

                                        var lec = new Lecturer() { Name = lecturerName.Trim() };
                                        long count = lecturers.FindAs<Lecturer>(Query<Lecturer>.EQ(x => x.Name, lec.Name)).Count();
                                        if (count == 0)
                                        {
                                            lecturers.Insert(lec);
                                            Console.WriteLine(lecturerName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
