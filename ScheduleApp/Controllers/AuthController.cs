using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScheduleData.Services;
using ScheduleData.Models;
using ScheduleApp.Infrastructure;
using ScheduleData.Exceptions;
using System.Web.Security;


namespace ScheduleApp.Controllers
{
    public class AuthController : Controller
    {
        UserService _userService;
        NotificationService _notificationService;

        public ActionResult Register(string mail, string name)
        {
            _userService = new UserService();

            try
            {
                var user = _userService.AddUser(mail, name);

                _notificationService = new NotificationService();

                _notificationService.SendEmail(mail, "Код подтверждения для приложения 'Рассписание ПетрГУ'",
                    "Ваш код подтвержения: " + user.Code);

                return new JsonNetResult("Письмо с кодом подтверждения отправлено на " + mail);
            }
            catch (UserAlreadyExsistException ex)
            {
                return new JsonNetResult("Этот Email уже зарегистрирован");
            }
            catch (IncorrectMailAddressException ex)
            {
                return new JsonNetResult("Некоректный почтовый адрес");
            }
            catch (IncorrectMailDomainException ex)
            {
                return new JsonNetResult("Почтовый домен не поддерживается");
            }

        }

    
        public ActionResult SignIn(string mail, string code)
        {
            _userService = new UserService();

            try
            {
                var user = _userService.GetUser(mail, code);
                FormsAuthentication.SetAuthCookie(mail, true);

                return new JsonNetResult(user);
            }
            catch (IncorrectAuthCodeException ex)
            {
                return new JsonNetResult("Неверный код подтверждения");
            }
            catch (UserNotFoundException ex)
            {
                return new JsonNetResult("Аккаунт не зарегистрирован");
            }
            catch (IncorrectMailAddressException ex)
            {
                return new JsonNetResult("Некоректный почтовый адрес");
            }
            catch (IncorrectMailDomainException ex)
            {
                return new JsonNetResult("Почтовый домен не поддерживается");
            }

            return null;
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return new JsonNetResult("Вы вышли из своей учетной записи");
        }
    }
}