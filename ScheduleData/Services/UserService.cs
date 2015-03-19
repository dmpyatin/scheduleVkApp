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
using ScheduleData.Infrastructure;

namespace ScheduleData.Services
{
    public class UserService
    {
        private MongoCollection<User> _users;

        public UserService()
        {
            var con = new MongoConnectionStringBuilder(
               ConfigurationManager.ConnectionStrings["MongoDbUsers"].ConnectionString);

            MongoClient client = new MongoClient(con.ConnectionString);
            MongoServer server = client.GetServer();

            var db = server.GetDatabase(con.DatabaseName);

            _users = db.GetCollection<User>("users");
        }


        public User GetUser(string mail, string code)
        {
            var util = new RegexUtilities();

            if (util.IsValidEmail(mail))
            {
                if (util.IsValidDomain(mail))
                {
                    var query = Query.And(
                Query<User>.Matches(x => x.Mail, mail),
                Query<User>.Matches(x => x.Code, code)
                );

                    var users = _users.Find(query).ToList();

                    if (users.Count == 0)
                    {

                        var query2 = Query<User>.Matches(x => x.Mail, mail);
                        var users2 = _users.Find(query2).ToList();
                        if (users2.Count == 0)
                        {
                            throw new UserNotFoundException();
                        }

                        throw new IncorrectAuthCodeException();
                    }

                    return users.FirstOrDefault();
                }
                else
                {
                    throw new IncorrectMailDomainException();
                }
            }
            else
            {
                throw new IncorrectMailAddressException();
            }
        }

        public UserSettings GetUserSettings(string mail)
        {
            var util = new RegexUtilities();

            if (util.IsValidEmail(mail))
            {
                if (util.IsValidDomain(mail))
                {
                    var query = Query<User>.Matches(x => x.Mail, mail);

                    var users = _users.Find(query).ToList();

                    var user = users.FirstOrDefault();

                    return user.Settings != null ? user.Settings : new UserSettings();
                }
                else
                {
                    throw new IncorrectMailDomainException();
                }
            }
            else
            {
                throw new IncorrectMailAddressException();
            }
        }



        public void SaveUserSettings(string mail, UserSettings us)
        {
            var util = new RegexUtilities();

            if (util.IsValidEmail(mail))
            {
                if (util.IsValidDomain(mail))
                {
                    var query = Query<User>.Matches(x => x.Mail, mail);

                    var users = _users.Find(query).ToList();

                    var user = users.FirstOrDefault();

                    var updUser = _users.FindOneById(user.Id);

                    updUser.Settings = us;

                    _users.Save(updUser);
                }
                else
                {
                    throw new IncorrectMailDomainException();
                }
            }
            else
            {
                throw new IncorrectMailAddressException();
            }
        }

        public User AddUser(string mail, string name)
        {
            var util = new RegexUtilities();

            if (util.IsValidEmail(mail))
            {
                if (util.IsValidDomain(mail))
                {
                    var query = Query<User>.Matches(x => x.Mail, mail);

                    var count = _users.Find(query).ToList().Count;
                    if (count > 0)
                    {
                        throw new UserAlreadyExsistException();
                    }

                    var user = new User()
                    {
                        Name = name,
                        Code = ((10000 + (Environment.TickCount & Int32.MaxValue)) % 100000).ToString(),
                        Mail = mail
                    };

                    _users.Insert(user);

                    return user;
                }
                else
                {
                    throw new IncorrectMailDomainException();
                }
            }
            else
            {
                throw new IncorrectMailAddressException();
            }       
        }

    }
}
