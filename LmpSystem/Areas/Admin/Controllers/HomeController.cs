using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LmpSystem.ViewModels;
using LmpSystem.Models;
using LmpSystem.Repository;

namespace LmpSystem.Areas.Admin.Controllers
{
    //[Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = db.UserRepository.FindByUsername(model.Username);
                if (user != null)
                {
                    if (user.Password == Common.Function.CalculateMD5Hash(model.Password) && user.Status == true)
                    {
                        SetCookie(user.Username, model.RememberMe, user.UserRole);
                        if (ReturnUrl != null)
                            return Redirect(ReturnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
                    return View();
                }
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public void SetCookie(string username, bool rememberme = false, string role = "User")
        {
            var authTicket = new FormsAuthenticationTicket(
                               1,
                               username,
                               DateTime.Now,
                               DateTime.Now.AddMinutes(120),
                               rememberme,
                               role
                               );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);
        }

        [Authorize]
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult ChangePass(UserChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.OldPassword == model.Password)
                {
                    ViewBag.anno = "Mật khẩu mới không được trùng mật khẩu cũ !";
                    return View();
                }
                else
                {
                    User user = db.UserRepository.FindByUsername(User.Identity.Name);
                    if (user != null)
                    {
                        user.Password = Common.Function.CalculateMD5Hash(model.Password);
                        db.Commit();
                        return RedirectToAction("Logout");
                    }
                }
            }
            return View();
        }

    }
}