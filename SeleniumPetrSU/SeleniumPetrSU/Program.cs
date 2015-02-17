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
    class Program
    {
        static void InitIO(string fname)
        {
            Console.SetOut(new StreamWriter(new FileStream(fname, FileMode.Create)));
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }

        static void Main(string[] args)
        {
            //InitIO("results.txt");
            var scheduleGrabber = new ScheduleGrabber();
            var mdbMigration = new MdbMigrator();

            scheduleGrabber.Grab();
            mdbMigration.Migrate();

        }
    }
}
