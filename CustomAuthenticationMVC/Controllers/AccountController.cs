using CustomAuthenticationMVC.CustomAuthentication;
using CustomAuthenticationMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using System.Web.UI.WebControls;

namespace CustomAuthenticationMVC.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        
        [HttpPost]      
        public ActionResult Login(User userObj, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                bool memberValid = Membership.ValidateUser(userObj.Username, userObj.Password);
                if (memberValid)
                {
                    var user = (CustomMembershipUser)Membership.GetUser(userObj.Username);
                    if (user != null)
                    {
                        User userModel = new Models.User()
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            //ne moze lista stringova u jedan string
                           //RoleName = user.RoleName
                           Role = user.RoleName
                        };
                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, userObj.Username, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );
                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                        Response.Cookies.Add(faCookie);
                    }
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Pogresno korisnicko ime ili lozinka ");
            return View(userObj);
        }
        
        [HttpGet]
        
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
       [CustomAuthorize(Roles="User")]
        public ActionResult Registration(User userOb)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;
            if (ModelState.IsValid)
            {

                string userName = Membership.GetUserNameByEmail(userOb.Email);
                if (!string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
                    return View(userOb);
                }
                //Save User Data 
                using (AuthenticationDB dbContext = new AuthenticationDB())
                {
                    var user = new User()
                    {
                        Username = userOb.Username,
                        FirstName = userOb.FirstName,
                        LastName = userOb.LastName,
                        Email = userOb.Email,
                        Password = userOb.Password,
                       // ActivationCode = Guid.NewGuid(),
                    };
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }

               // VerificationEmail(userOb.Email, userOb.ActivationCode.ToString());
                messageRegistration = "Your account has been created successfully. ^_^";
                statusRegistration = true;
            }
            else
            {
                messageRegistration = "Something Wrong!";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;
            return View(userOb);
        }
        [HttpGet]
       
        public ActionResult ActivationAccount(string id)
        {
            bool statusAccount = false;
            using (AuthenticationDB dbContext = new AuthenticationDB())
            {
                var userAccount = dbContext.Users.Where(u => u.ActivationCode.ToString().Equals(id)).FirstOrDefault();
                if (userAccount != null)
                {
                    userAccount.IsActive = true;
                    dbContext.SaveChanges();
                    statusAccount = true;
                }
                else
                {
                    ViewBag.Message = "Something Wrong !!";
                }
            }
            ViewBag.Status = statusAccount;
            return View();
        }
        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }
        /*
        [NonAction]
        [AllowAnonymous]
        public void VerificationEmail(string email, string activationCode)
        {
            var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
            var fromEmail = new MailAddress("mehdi.rami2012@gmail.com", "Activation Account - AKKA");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "******************";
            string subject = "Activation Account !";
            string body = "Please click on the following link in order to activate your account" + "< a href = "+"> Activation Account! </ a > ";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        
        }
        */
    }
}